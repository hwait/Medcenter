using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using ServiceStack;

namespace SyncronizationTask
{
    class Program
    {
        static SyncLog _syncLog=new SyncLog();
        private static SyncLog _prevSyncLog;
        static private SqlConnection _conn;
        static DateTime _dt;
        static JsonServiceClient _cli = new JsonServiceClient("http://Nikk-PC/Medcenter.Service.MVC5/api/");

        private static void Main(string[] args)
        {
            TimeSpan dur;

            List<SyncStructure> syncStructures = new List<SyncStructure>();
            List<SyncRelationsStructure> syncRelationsStructures = new List<SyncRelationsStructure>();
            List<RemoveItem> syncRemoveItems = new List<RemoveItem>();
            var fn = "rv.txt";

            //byte[] rv = (File.Exists(fn)) ? File.ReadAllBytes(fn) : new byte[8];

            _conn =
                new SqlConnection(@"Data Source=NIKK-PC\SQLEXPRESS;Initial Catalog=Medcenter;Integrated Security=True");
            _conn.Open();

            #region 1.1. Get Client MT

            _dt = DateTime.Now;
            SqlCommand cmd = new SqlCommand("synco_Main", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nmb", 0);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    syncStructures.Add(new SyncStructure(rdr));
                }
            }
            _syncLog.ClientMainGetDuration = (DateTime.Now - _dt).Milliseconds;

            #endregion

            #region 1.2. Get Client RT

            _dt = DateTime.Now;
            cmd = new SqlCommand("synco_All", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nmb", 0);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    syncRelationsStructures.Add(new SyncRelationsStructure(rdr));
                }
            }
            _syncLog.ClientRelationsGetDuration = (DateTime.Now - _dt).Milliseconds;

            #endregion

            #region 1.3. Get Client RemoveList

            cmd = new SqlCommand("synco_Del", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nmb", 0);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    syncRemoveItems.Add(new RemoveItem(rdr));
                }
            }

            #endregion

            #region 1.4. Get Client old Sync Row and Fix @@DBTS

            cmd = new SqlCommand("synco_rv", _conn);
            cmd.CommandType = CommandType.StoredProcedure;

            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                _prevSyncLog = (rdr.Read()) ? new SyncLog(rdr) : new SyncLog();
            }

            #endregion

            _conn.Close();

            // 1.5

            var mts = new MainTablesSync
            {
                Cid = 1,
                OldSyncLog = _prevSyncLog,
                SyncStructures = syncStructures,
                SyncRelationsStructures = syncRelationsStructures,
                SyncRemoveItems = syncRemoveItems
            };

            _syncLog.ClientMainSendBytes = mts.ToJson().Length;

            // 1.6

            var rtask = _cli.PostAsync(mts);

            try
            {
                Task.WaitAll(rtask);
                var r = rtask.Result;
                _syncLog.AllMainDuration = (DateTime.Now - _dt).Milliseconds;
                _syncLog.ClientMainGetBytes = r.ToJson().Length;
                _syncLog.ServerMainGetDuration = r.MainGetDur;
                _syncLog.ServerMainPutDuration = r.MainPutDur;
                _syncLog.ServerRelationsGetDuration = r.RelGetDur;
                _syncLog.ServerRelationsPutDuration = r.RelPutDur;
                DataReceived(r.Ids, r.MainStructures, r.RelationsStructures, r.RemoveStructures);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }

        private static void DataReceived(List<SyncId> ids, List<SyncStructure> mainStructures, List<SyncRelationsStructure> relationsStructures, List<RemoveItem> removeStructures)
        {
            // 3.1 Data Recieved
            List<SyncStructure> lastChangedMT = new List<SyncStructure>();
            List<SyncRelationsStructure> lastChangedRT = new List<SyncRelationsStructure>();
            List<RemoveItem> lastChangedDT = new List<RemoveItem>();

            SqlCommand cmd, cmd1;
            string comstr;
            List<SyncRelationsStructure> syncRelationsStructures=new List<SyncRelationsStructure>();
            
            _conn.Open();

            // 3.2 

            #region Fix Crv2

            cmd1 = new SqlCommand("WITH CTE AS ( SELECT TOP 1 * FROM SyncLog ORDER BY ts DESC) UPDATE CTE SET Crv1=@@DBTS,Crv2=@@DBTS", _conn);
            cmd1.CommandType = CommandType.Text;
            cmd1.ExecuteNonQuery();

            #endregion
            
            #region Get lastChangedMT

            _dt = DateTime.Now;
            cmd = new SqlCommand("synco_Main", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nmb", 1);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    lastChangedMT.Add(new SyncStructure(rdr));
                }
            }
            _syncLog.ClientMainGetDuration = (DateTime.Now - _dt).Milliseconds;

            #endregion

            #region Get lastChangedRT

            _dt = DateTime.Now;
            cmd = new SqlCommand("synco_All", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nmb", 1);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    lastChangedRT.Add(new SyncRelationsStructure(rdr));
                }
            }
            _syncLog.ClientRelationsGetDuration = (DateTime.Now - _dt).Milliseconds;

            #endregion

            #region Get lastChangedDT

            cmd = new SqlCommand("synco_Del", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nmb", 1);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    lastChangedDT.Add(new RemoveItem(rdr));
                }
            }

            #endregion
            
            // 3.3 Collisions
            syncRelationsStructures = relationsStructures;

            // 3.4 Block DB

            #region 3.5 Update Client DB with Sids
            
            _dt = DateTime.Now;
            foreach (var s in ids)
            {
                switch (s.Tt)
                {
                    case 1:
                        cmd = new SqlCommand("update Cities set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                    case 28:
                        cmd = new SqlCommand("update UserAuth set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                    case 26:
                        cmd = new SqlCommand("update Surveys set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                    case 24:
                        cmd = new SqlCommand("update Schedules set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                    case 23:
                        cmd = new SqlCommand("update Receptions set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                    case 22:
                        cmd = new SqlCommand("update Positions set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                    case 21:
                        cmd = new SqlCommand("update Phrases set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                    case 20:
                        cmd = new SqlCommand("update Payments set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                    case 19:
                        cmd = new SqlCommand("update Patterns set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                    case 17:
                        cmd = new SqlCommand("update Patients set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                    case 12:
                        cmd = new SqlCommand("update Paraphrases set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                    case 16:
                        cmd = new SqlCommand("update Packages set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                    case 11:
                        cmd = new SqlCommand("update PackageGroups set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                    case 8:
                        cmd = new SqlCommand("update Inspections set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                    case 6:
                        cmd = new SqlCommand("update Doctors set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                    case 2:
                        cmd = new SqlCommand("update Discounts set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Sid", s.Sid);
                        cmd.Parameters.AddWithValue("@Id", s.Id);
                        cmd.ExecuteNonQuery();
                        break;
                }
            }
            _syncLog.ClientMainIdsUpdateDuration = (DateTime.Now - _dt).Milliseconds;
            
            #endregion

            #region 3.6 Update Client DB with Main
            
            _dt = DateTime.Now;
            foreach (var ss in mainStructures)
            {
                switch (ss.Tt)
                {
                    case 1:
                        cmd = new SqlCommand("synci_Cities", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.ExecuteNonQuery();
                        break;
                    case 28:
                        cmd = new SqlCommand("synci_UserAuth", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.Parameters.AddWithValue("@P2", ss.P2);
                        cmd.Parameters.AddWithValue("@P3", ss.P3);
                        cmd.Parameters.AddWithValue("@P4", ss.P4);
                        cmd.Parameters.AddWithValue("@P5", ss.P5);
                        cmd.Parameters.AddWithValue("@P6", ss.P6);
                        cmd.Parameters.AddWithValue("@P7", ss.P7);
                        cmd.Parameters.AddWithValue("@P8", ss.P8);
                        cmd.Parameters.AddWithValue("@P9", ss.P9);
                        cmd.Parameters.AddWithValue("@P10", ss.P10);
                        cmd.Parameters.AddWithValue("@P11", ss.P11);
                        cmd.Parameters.AddWithValue("@P12", ss.P12);
                        cmd.Parameters.AddWithValue("@P13", ss.P13);
                        cmd.Parameters.AddWithValue("@P14", ss.P14);
                        cmd.Parameters.AddWithValue("@P15", ss.P15);
                        cmd.ExecuteNonQuery();
                        break;
                    case 26:
                        cmd = new SqlCommand("synci_Surveys", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.Parameters.AddWithValue("@P2", ss.P2);
                        cmd.ExecuteNonQuery();
                        break;
                    case 24:
                        cmd = new SqlCommand("synci_Schedules", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.Parameters.AddWithValue("@P2", ss.P2);
                        cmd.Parameters.AddWithValue("@P3", ss.P3);
                        cmd.Parameters.AddWithValue("@P4", ss.P4);
                        cmd.ExecuteNonQuery();
                        break;
                    case 23:
                        cmd = new SqlCommand("synci_Receptions", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.Parameters.AddWithValue("@P2", ss.P2);
                        cmd.Parameters.AddWithValue("@P3", ss.P3);
                        cmd.Parameters.AddWithValue("@P4", ss.P4);
                        cmd.Parameters.AddWithValue("@P5", ss.P5);
                        cmd.ExecuteNonQuery();
                        break;
                    case 22:
                        cmd = new SqlCommand("synci_Positions", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.ExecuteNonQuery();
                        break;
                    case 21:
                        cmd = new SqlCommand("synci_Phrases", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.Parameters.AddWithValue("@P2", ss.P2);
                        cmd.Parameters.AddWithValue("@P3", ss.P3);
                        cmd.Parameters.AddWithValue("@P4", ss.P4);
                        cmd.Parameters.AddWithValue("@P5", ss.P5);
                        cmd.ExecuteNonQuery();
                        break;
                    case 20:
                        cmd = new SqlCommand("synci_Payments", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.Parameters.AddWithValue("@P2", ss.P2);
                        cmd.Parameters.AddWithValue("@P3", ss.P3);
                        cmd.Parameters.AddWithValue("@P4", ss.P4);
                        cmd.ExecuteNonQuery();
                        break;
                    case 19:
                        cmd = new SqlCommand("synci_Patterns", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.Parameters.AddWithValue("@P2", ss.P2);
                        cmd.Parameters.AddWithValue("@P3", ss.P3);
                        cmd.ExecuteNonQuery();
                        break;
                    case 17:
                        cmd = new SqlCommand("synci_Patients", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.Parameters.AddWithValue("@P2", ss.P2);
                        cmd.Parameters.AddWithValue("@P3", ss.P3);
                        cmd.Parameters.AddWithValue("@P4", ss.P4);
                        cmd.Parameters.AddWithValue("@P5", ss.P5);
                        cmd.Parameters.AddWithValue("@P6", ss.P6);
                        cmd.Parameters.AddWithValue("@P7", ss.P7);
                        cmd.Parameters.AddWithValue("@P8", ss.P8);
                        cmd.Parameters.AddWithValue("@P9", ss.P9);
                        cmd.Parameters.AddWithValue("@P10", ss.P10);
                        cmd.ExecuteNonQuery();
                        break;
                    case 16:
                        cmd = new SqlCommand("synci_Paraphrases", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.Parameters.AddWithValue("@P2", ss.P2);
                        cmd.Parameters.AddWithValue("@P3", ss.P3);
                        cmd.Parameters.AddWithValue("@P4", ss.P4);
                        cmd.Parameters.AddWithValue("@P5", ss.P5);
                        cmd.Parameters.AddWithValue("@P6", ss.P6);
                        cmd.ExecuteNonQuery();
                        break;
                    case 12:
                        cmd = new SqlCommand("synci_Packages", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.Parameters.AddWithValue("@P2", ss.P2);
                        cmd.Parameters.AddWithValue("@P3", ss.P3);
                        cmd.ExecuteNonQuery();
                        break;
                    case 11:
                        cmd = new SqlCommand("synci_PackageGroups", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.Parameters.AddWithValue("@P2", ss.P2);
                        cmd.Parameters.AddWithValue("@P3", ss.P3);
                        cmd.ExecuteNonQuery();
                        break;
                    case 8:
                        cmd = new SqlCommand("synci_Inspections", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.Parameters.AddWithValue("@P2", ss.P2);
                        cmd.ExecuteNonQuery();
                        break;
                    case 6:
                        cmd = new SqlCommand("synci_Doctors", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.Parameters.AddWithValue("@P2", ss.P2);
                        cmd.Parameters.AddWithValue("@P3", ss.P3);
                        cmd.ExecuteNonQuery();
                        break;
                    case 2:
                        cmd = new SqlCommand("synci_Discounts", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.Parameters.AddWithValue("@P2", ss.P2);
                        cmd.Parameters.AddWithValue("@P3", ss.P3);
                        cmd.Parameters.AddWithValue("@P4", ss.P4);
                        cmd.ExecuteNonQuery();
                        break;
                    case 7:
                        cmd = new SqlCommand("synci_Headers", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sid", ss.Id);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.ExecuteNonQuery();
                        break;
                }
            }
            _syncLog.ClientMainPutDuration = (DateTime.Now - _dt).Milliseconds;

            #endregion
            
            #region 3.7 Update Client DB with Relations

            foreach (var ss in syncRelationsStructures)
            {
                switch (ss.Tt)
                {
                    case 25:
                        cmd = new SqlCommand(
                            "synci_SurveyPhrases", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id0", ss.Id0);
                        cmd.Parameters.AddWithValue("@Id1", ss.Id1);
                        cmd.ExecuteNonQuery();
                        break;
                    case 15:
                        cmd = new SqlCommand(
                            "synci_PackagesInReception", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id0", ss.Id0);
                        cmd.Parameters.AddWithValue("@Id1", ss.Id1);
                        cmd.ExecuteNonQuery();
                        break;
                    case 14:
                        cmd = new SqlCommand(
                            "synci_PackagesInGroups", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id0", ss.Id0);
                        cmd.Parameters.AddWithValue("@Id1", ss.Id1);
                        cmd.ExecuteNonQuery();
                        break;
                    case 13:
                        cmd = new SqlCommand(
                            "synci_PackagesInDoctors", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id0", ss.Id0);
                        cmd.Parameters.AddWithValue("@Id1", ss.Id1);
                        cmd.ExecuteNonQuery();
                        break;
                    case 10:
                        cmd = new SqlCommand(
                            "synci_NursesInDoctors", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id0", ss.Id0);
                        cmd.Parameters.AddWithValue("@Id1", ss.Id1);
                        cmd.ExecuteNonQuery();
                        break;
                    case 9:
                        cmd = new SqlCommand(
                            "synci_InspectionsInPackages", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id0", ss.Id0);
                        cmd.Parameters.AddWithValue("@Id1", ss.Id1);
                        cmd.ExecuteNonQuery();
                        break;
                    case 5:
                        cmd = new SqlCommand(
                            "synci_DoctorPatterns", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id0", ss.Id0);
                        cmd.Parameters.AddWithValue("@Id1", ss.Id1);
                        cmd.ExecuteNonQuery();
                        break;
                    case 3:
                        cmd = new SqlCommand(
                            "synci_DiscountsInPackages", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id0", ss.Id0);
                        cmd.Parameters.AddWithValue("@Id1", ss.Id1);
                        cmd.ExecuteNonQuery();
                        break;
                    case 4:
                        cmd = new SqlCommand(
                            "synci_DoctorInspectionParaphrases", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id0", ss.Id0);
                        cmd.Parameters.AddWithValue("@Id1", ss.Id1);
                        cmd.Parameters.AddWithValue("@Id2", ss.Id2);
                        cmd.ExecuteNonQuery();
                        break;
                    case 18:
                        cmd = new SqlCommand(
                            "synci_PatternPositions", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id0", ss.Id0);
                        cmd.Parameters.AddWithValue("@Id1", ss.Id1);
                        cmd.Parameters.AddWithValue("@Id2", ss.Id2);
                        cmd.Parameters.AddWithValue("@P0", ss.P0);
                        cmd.Parameters.AddWithValue("@P1", ss.P1);
                        cmd.Parameters.AddWithValue("@P2", ss.P2);
                        cmd.ExecuteNonQuery();
                        break;
                }
            }

            #endregion

            // 3.8 Remove rows

            // 3.9 Deblock DB

            #region 3.10 Save SyncLog, fix RV

            cmd =
                new SqlCommand(
                    "EXEC synci_SyncLog  @durCliMainGet,@durCliRelGet,@durCliMainPut,@durCliRelPut,@durSrvMainGet,@durSrvRelGet,@durSrvMainPut,@durSrvRelPut,@durAllMain," +
                    "@bytesMainSend,@bytesMainGet,@bytesRelSend,@bytesRelGet,@durAllRel,@durCliIdsPut", _conn);
            cmd.Parameters.AddWithValue("@durCliMainGet", _syncLog.ClientMainGetDuration);
            cmd.Parameters.AddWithValue("@durCliRelGet", _syncLog.ClientRelationsGetDuration);
            cmd.Parameters.AddWithValue("@durCliMainPut", _syncLog.ClientMainPutDuration);
            cmd.Parameters.AddWithValue("@durCliRelPut", _syncLog.ClientRelationsPutDuration);
            cmd.Parameters.AddWithValue("@durSrvMainGet", _syncLog.ServerMainGetDuration);
            cmd.Parameters.AddWithValue("@durSrvRelGet", _syncLog.ServerRelationsGetDuration);
            cmd.Parameters.AddWithValue("@durSrvMainPut", _syncLog.ServerMainPutDuration);
            cmd.Parameters.AddWithValue("@durSrvRelPut", _syncLog.ServerRelationsPutDuration);
            cmd.Parameters.AddWithValue("@durAllMain", _syncLog.AllMainDuration);
            cmd.Parameters.AddWithValue("@bytesMainSend", _syncLog.ClientMainSendBytes);
            cmd.Parameters.AddWithValue("@bytesMainGet", _syncLog.ClientMainGetBytes);
            cmd.Parameters.AddWithValue("@bytesRelSend", _syncLog.ClientRelationsSendBytes);
            cmd.Parameters.AddWithValue("@bytesRelGet", _syncLog.ClientRelationsGetDuration);
            cmd.Parameters.AddWithValue("@durAllRel", _syncLog.AllRelationsDuration);
            cmd.Parameters.AddWithValue("@durCliIdsPut", _syncLog.ClientMainIdsUpdateDuration);
            cmd.ExecuteNonQuery();

            #endregion

            // 3.11 Save Changed Tables

            _conn.Close();
        }
    }
}
