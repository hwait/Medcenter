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
        static void Main(string[] args)
        {
            TimeSpan dur;
           
            List<SyncStructure> syncStructures=new List<SyncStructure>();
            List<SyncRelationsStructure> syncRelationsStructures = new List<SyncRelationsStructure>();
            List<RemoveItem> syncRemoveItems = new List<RemoveItem>();
            var fn = "rv.txt";
            
            //byte[] rv = (File.Exists(fn)) ? File.ReadAllBytes(fn) : new byte[8];

            _conn = new SqlConnection(@"Data Source=NIKK-PC\SQLEXPRESS;Initial Catalog=Medcenter;Integrated Security=True");
            _conn.Open();

            #region 1.1. Get Client MT

            _dt = DateTime.Now;
            SqlCommand cmd = new SqlCommand("synco_Main", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add(new SqlParameter("@rv", BitConverter.GetBytes(_prevSyncLog.Crv)));
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
            //cmd.Parameters.Add(new SqlParameter("@rv", _prevSyncLog.Crv));
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
            //cmd.Parameters.Add(new SqlParameter("@rv", _prevSyncLog.Crv));
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

            var mts = new MainTablesSync { Cid = 1, OldSyncLog = _prevSyncLog, SyncStructures = syncStructures, SyncRelationsStructures = syncRelationsStructures, SyncRemoveItems = syncRemoveItems };

            _syncLog.ClientMainSendBytes=mts.ToJson().Length;

            // 1.6

            var rtask=_cli.PostAsync(mts);

            try
            {
                Task.WaitAll(rtask);
                var r = rtask.Result;
                _syncLog.AllMainDuration = (DateTime.Now - _dt).Milliseconds;
                _syncLog.ClientMainGetBytes = r.ToJson().Length;
                _syncLog.ServerMainGetDuration = r.DurGet;
                _syncLog.ServerMainPutDuration = r.DurPut;
                _syncLog.Srv = r.Srv;
                MainTablesReceived(r.Srv, r.Ids, r.MainStructures);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
            
            // 3.1
            
            //_cli.PostAsync(mts)
            //        .Success(r =>
            //        {
            //            // 3.1
            //            _syncLog.AllMainDuration = (DateTime.Now - _dt).Milliseconds;
            //            _syncLog.ClientMainGetBytes=r.ToJson().Length;
            //            _syncLog.ServerMainGetDuration = r.DurGet;
            //            _syncLog.ServerMainPutDuration = r.DurPut;
            //            _syncLog.Srv = r.Srv;
            //            MainTablesReceived(r.Srv,r.Ids, r.MainStructures);
            //        })
            //        .Error(ex =>
            //        {
            //            throw ex;
            //        });
            }

        private static void MainTablesReceived(byte[] srv, List<SyncId> ids, List<SyncStructure> mainStructures)
        {
            
            SqlCommand cmd;
            string comstr;
            List<SyncRelationsStructure> syncRelationsStructures=new List<SyncRelationsStructure>();
            _conn.Open();
            
            // 3.2
            #region Update Client with Sids
            
            _dt = DateTime.Now;
            foreach (var s in ids)
            {
                switch (s.TableType)
                {
                    case 1:
                        cmd = new SqlCommand("update Cities set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                    case 28:
                        cmd = new SqlCommand("update UserAuth set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                    case 26:
                        cmd = new SqlCommand("update Surveys set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                    case 24:
                        cmd = new SqlCommand("update Schedules set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                    case 23:
                        cmd = new SqlCommand("update Receptions set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                    case 22:
                        cmd = new SqlCommand("update Positions set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                    case 21:
                        cmd = new SqlCommand("update Phrases set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                    case 20:
                        cmd = new SqlCommand("update Payments set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                    case 19:
                        cmd = new SqlCommand("update Patterns set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                    case 17:
                        cmd = new SqlCommand("update Patients set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                    case 12:
                        cmd = new SqlCommand("update Paraphrases set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                    case 16:
                        cmd = new SqlCommand("update Packages set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                    case 11:
                        cmd = new SqlCommand("update PackageGroups set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                    case 8:
                        cmd = new SqlCommand("update Inspections set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                    case 6:
                        cmd = new SqlCommand("update Doctors set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                    case 2:
                        cmd = new SqlCommand("update Discounts set Sid=@Sid where Id=@Id", _conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@Sid", s.Sid));
                        cmd.Parameters.Add(new SqlParameter("@Id", s.Id));
                        cmd.ExecuteNonQuery();
                        break;
                }
            }
            _syncLog.ClientMainIdsUpdateDuration = (DateTime.Now - _dt).Milliseconds;
            
            #endregion

            // 3.3
            #region Update Client with Main
            
            _dt = DateTime.Now;
            foreach (var ss in mainStructures)
            {
                switch (ss.TableType)
                {
                    case 1:
                        cmd = new SqlCommand("synci_Cities", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Sid", ss.Sid));
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.ExecuteNonQuery();
                        break;
                    case 28:
                        cmd = new SqlCommand("synci_UserAuth", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Sid", ss.Sid));
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.Parameters.Add(new SqlParameter("@P2", ss.P2));
                        cmd.Parameters.Add(new SqlParameter("@P3", ss.P3));
                        cmd.Parameters.Add(new SqlParameter("@P4", ss.P4));
                        cmd.Parameters.Add(new SqlParameter("@P5", ss.P5));
                        cmd.Parameters.Add(new SqlParameter("@P6", ss.P6));
                        cmd.Parameters.Add(new SqlParameter("@P7", ss.P7));
                        cmd.Parameters.Add(new SqlParameter("@P8", ss.P8));
                        cmd.Parameters.Add(new SqlParameter("@P9", ss.P9));
                        cmd.Parameters.Add(new SqlParameter("@P10", ss.P10));
                        cmd.Parameters.Add(new SqlParameter("@P11", ss.P11));
                        cmd.Parameters.Add(new SqlParameter("@P12", ss.P12));
                        cmd.Parameters.Add(new SqlParameter("@P13", ss.P13));
                        cmd.Parameters.Add(new SqlParameter("@P14", ss.P14));
                        cmd.Parameters.Add(new SqlParameter("@P15", ss.P15));
                        cmd.ExecuteNonQuery();
                        break;
                    case 26:
                        cmd = new SqlCommand("synci_Surveys", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.Parameters.Add(new SqlParameter("@P2", ss.P2));
                        cmd.ExecuteNonQuery();
                        break;
                    case 24:
                        cmd = new SqlCommand("synci_Schedules", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.Parameters.Add(new SqlParameter("@P2", ss.P2));
                        cmd.Parameters.Add(new SqlParameter("@P3", ss.P3));
                        cmd.Parameters.Add(new SqlParameter("@P4", ss.P4));
                        cmd.ExecuteNonQuery();
                        break;
                    case 23:
                        cmd = new SqlCommand("synci_Receptions", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.Parameters.Add(new SqlParameter("@P2", ss.P2));
                        cmd.Parameters.Add(new SqlParameter("@P3", ss.P3));
                        cmd.Parameters.Add(new SqlParameter("@P4", ss.P4));
                        cmd.Parameters.Add(new SqlParameter("@P5", ss.P5));
                        cmd.ExecuteNonQuery();
                        break;
                    case 22:
                        cmd = new SqlCommand("synci_Positions", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.ExecuteNonQuery();
                        break;
                    case 21:
                        cmd = new SqlCommand("synci_Phrases", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.Parameters.Add(new SqlParameter("@P2", ss.P2));
                        cmd.Parameters.Add(new SqlParameter("@P3", ss.P3));
                        cmd.Parameters.Add(new SqlParameter("@P4", ss.P4));
                        cmd.Parameters.Add(new SqlParameter("@P5", ss.P5));
                        cmd.ExecuteNonQuery();
                        break;
                    case 20:
                        cmd = new SqlCommand("synci_Payments", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.Parameters.Add(new SqlParameter("@P2", ss.P2));
                        cmd.Parameters.Add(new SqlParameter("@P3", ss.P3));
                        cmd.Parameters.Add(new SqlParameter("@P4", ss.P4));
                        cmd.ExecuteNonQuery();
                        break;
                    case 19:
                        cmd = new SqlCommand("synci_Patterns", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.Parameters.Add(new SqlParameter("@P2", ss.P2));
                        cmd.Parameters.Add(new SqlParameter("@P3", ss.P3));
                        cmd.ExecuteNonQuery();
                        break;
                    case 17:
                        cmd = new SqlCommand("synci_Patients", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.Parameters.Add(new SqlParameter("@P2", ss.P2));
                        cmd.Parameters.Add(new SqlParameter("@P3", ss.P3));
                        cmd.Parameters.Add(new SqlParameter("@P4", ss.P4));
                        cmd.Parameters.Add(new SqlParameter("@P5", ss.P5));
                        cmd.Parameters.Add(new SqlParameter("@P6", ss.P6));
                        cmd.Parameters.Add(new SqlParameter("@P7", ss.P7));
                        cmd.Parameters.Add(new SqlParameter("@P8", ss.P8));
                        cmd.Parameters.Add(new SqlParameter("@P9", ss.P9));
                        cmd.Parameters.Add(new SqlParameter("@P10", ss.P10));
                        cmd.ExecuteNonQuery();
                        break;
                    case 12:
                        cmd = new SqlCommand("synci_Paraphrases", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.Parameters.Add(new SqlParameter("@P2", ss.P2));
                        cmd.Parameters.Add(new SqlParameter("@P3", ss.P3));
                        cmd.Parameters.Add(new SqlParameter("@P4", ss.P4));
                        cmd.Parameters.Add(new SqlParameter("@P5", ss.P5));
                        cmd.Parameters.Add(new SqlParameter("@P6", ss.P6));
                        cmd.ExecuteNonQuery();
                        break;
                    case 16:
                        cmd = new SqlCommand("synci_Packages", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.Parameters.Add(new SqlParameter("@P2", ss.P2));
                        cmd.Parameters.Add(new SqlParameter("@P3", ss.P3));
                        cmd.ExecuteNonQuery();
                        break;
                    case 11:
                        cmd = new SqlCommand("synci_PackageGroups", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.Parameters.Add(new SqlParameter("@P2", ss.P2));
                        cmd.Parameters.Add(new SqlParameter("@P3", ss.P3));
                        cmd.ExecuteNonQuery();
                        break;
                    case 8:
                        cmd = new SqlCommand("synci_Inspections", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.Parameters.Add(new SqlParameter("@P2", ss.P2));
                        cmd.ExecuteNonQuery();
                        break;
                    case 6:
                        cmd = new SqlCommand("synci_Doctors", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.Parameters.Add(new SqlParameter("@P2", ss.P2));
                        cmd.Parameters.Add(new SqlParameter("@P3", ss.P3));
                        cmd.ExecuteNonQuery();
                        break;
                    case 2:
                        cmd = new SqlCommand("synci_Discounts", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.Parameters.Add(new SqlParameter("@P2", ss.P2));
                        cmd.Parameters.Add(new SqlParameter("@P3", ss.P3));
                        cmd.Parameters.Add(new SqlParameter("@P4", ss.P4));
                        cmd.ExecuteNonQuery();
                        break;
                }
            }
            _syncLog.ClientMainPutDuration = (DateTime.Now - _dt).Milliseconds;

            #endregion
            
            // 3.4
            #region Get Client Relations
            
            _dt = DateTime.Now;
            cmd = new SqlCommand("synco_All", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@rv", _prevSyncLog.Crv));
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    syncRelationsStructures.Add(new SyncRelationsStructure(rdr));
                }
            }
            _syncLog.ClientRelationsGetDuration = (DateTime.Now - _dt).Milliseconds;
            #endregion
            
            _conn.Close();
            
            // 3.5
            var mts = new RelationsSync {Cid = 1, Srv=_syncLog.Srv, SyncRelationsStructures = syncRelationsStructures};
            _syncLog.ClientRelationsSendBytes=mts.ToJson().Length;

            // 3.6
            _cli.PostAsync(mts)
                    .Success(r =>
                    {
                        // 5.1
                        _syncLog.AllMainDuration = (DateTime.Now - _dt).Milliseconds;
                        _syncLog.ClientRelationsGetBytes=r.ToJson().Length;
                        _syncLog.ServerRelationsGetDuration = r.DurGet;
                        _syncLog.ServerRelationsPutDuration = r.DurPut;
                        RelationsReceived(r.SyncRelationsStructures);
                    })
                    .Error(ex =>
                    {
                        throw ex;
                    });
            }

        private static void RelationsReceived(List<SyncRelationsStructure> syncRelationsStructures)
        {
            SqlCommand cmd;
            // 5.2

            #region SyncRelationsStructures

            foreach (var ss in syncRelationsStructures)
            {
                switch (ss.TableType)
                {
                    case 25:
                        cmd = new SqlCommand(
                            "EXEC synci_SurveyPhrases @Id0,@Id1", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id0", ss.Id0));
                        cmd.Parameters.Add(new SqlParameter("@Id1", ss.Id1));
                        cmd.ExecuteNonQuery();
                        break;
                    case 15:
                        cmd = new SqlCommand(
                            "EXEC synci_PackagesInReception @Id0,@Id1", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id0", ss.Id0));
                        cmd.Parameters.Add(new SqlParameter("@Id1", ss.Id1));
                        cmd.ExecuteNonQuery();
                        break;
                    case 14:
                        cmd = new SqlCommand(
                            "EXEC synci_PackagesInGroups @Id0,@Id1", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id0", ss.Id0));
                        cmd.Parameters.Add(new SqlParameter("@Id1", ss.Id1));
                        cmd.ExecuteNonQuery();
                        break;
                    case 13:
                        cmd = new SqlCommand(
                            "EXEC synci_PackagesInDoctors @Id0,@Id1", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id0", ss.Id0));
                        cmd.Parameters.Add(new SqlParameter("@Id1", ss.Id1));
                        cmd.ExecuteNonQuery();
                        break;
                    case 10:
                        cmd = new SqlCommand(
                            "EXEC synci_NursesInDoctors @Id0,@Id1", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id0", ss.Id0));
                        cmd.Parameters.Add(new SqlParameter("@Id1", ss.Id1));
                        cmd.ExecuteNonQuery();
                        break;
                    case 9:
                        cmd = new SqlCommand(
                            "EXEC synci_InspectionsInPackages @Id0,@Id1", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id0", ss.Id0));
                        cmd.Parameters.Add(new SqlParameter("@Id1", ss.Id1));
                        cmd.ExecuteNonQuery();
                        break;
                    case 5:
                        cmd = new SqlCommand(
                            "EXEC synci_DoctorPatterns @Id0,@Id1", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id0", ss.Id0));
                        cmd.Parameters.Add(new SqlParameter("@Id1", ss.Id1));
                        cmd.ExecuteNonQuery();
                        break;
                    case 3:
                        cmd = new SqlCommand(
                            "EXEC synci_DiscountsInPackages @Id0,@Id1", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id0", ss.Id0));
                        cmd.Parameters.Add(new SqlParameter("@Id1", ss.Id1));
                        cmd.ExecuteNonQuery();
                        break;
                    case 4:
                        cmd = new SqlCommand(
                            "EXEC synci_DoctorInspectionParaphrases @Id0,@Id1,@Id2", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id0", ss.Id0));
                        cmd.Parameters.Add(new SqlParameter("@Id1", ss.Id1));
                        cmd.Parameters.Add(new SqlParameter("@Id2", ss.Id2));
                        cmd.ExecuteNonQuery();
                        break;
                    case 18:
                        cmd = new SqlCommand(
                            "EXEC synci_PatternPositions @Id0,@Id1,@Id2,@P0,@P1,@P2", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id0", ss.Id0));
                        cmd.Parameters.Add(new SqlParameter("@Id1", ss.Id1));
                        cmd.Parameters.Add(new SqlParameter("@Id2", ss.Id2));
                        cmd.Parameters.Add(new SqlParameter("@P0", ss.P0));
                        cmd.Parameters.Add(new SqlParameter("@P1", ss.P1));
                        cmd.Parameters.Add(new SqlParameter("@P2", ss.P2));
                        cmd.ExecuteNonQuery();
                        break;
                }
            }

            #endregion

            // 5.3
            cmd =
                new SqlCommand(
                    "EXEC synci_SyncLog  @durCliMainGet,@durCliRelGet,@durCliMainPut,@durCliRelPut,@durSrvMainGet,@durSrvRelGet,@durSrvMainPut,@durSrvRelPut,@durAllMain," +
                    "@srv,@crv,@bytesMainSend,@bytesMainGet,@bytesRelSend,@bytesRelGet,@durAllRel,@durCliIdsPut", _conn);
            cmd.Parameters.Add(new SqlParameter("@durCliMainGet", _syncLog.ClientMainGetDuration));
            cmd.Parameters.Add(new SqlParameter("@durCliRelGet", _syncLog.ClientRelationsGetDuration));
            cmd.Parameters.Add(new SqlParameter("@durCliMainPut", _syncLog.ClientMainPutDuration));
            cmd.Parameters.Add(new SqlParameter("@durCliRelPut", _syncLog.ClientRelationsPutDuration));
            cmd.Parameters.Add(new SqlParameter("@durSrvMainGet", _syncLog.ServerMainGetDuration));
            cmd.Parameters.Add(new SqlParameter("@durSrvRelGet", _syncLog.ServerRelationsGetDuration));
            cmd.Parameters.Add(new SqlParameter("@durSrvMainPut", _syncLog.ServerMainPutDuration));
            cmd.Parameters.Add(new SqlParameter("@durSrvRelPut", _syncLog.ServerRelationsPutDuration));
            cmd.Parameters.Add(new SqlParameter("@durAllMain", _syncLog.AllMainDuration));
            cmd.Parameters.Add(new SqlParameter("@srv", _syncLog.Srv));
            cmd.Parameters.Add(new SqlParameter("@crv", _syncLog.Crv));
            cmd.Parameters.Add(new SqlParameter("@bytesMainSend", _syncLog.ClientMainSendBytes));
            cmd.Parameters.Add(new SqlParameter("@bytesMainGet", _syncLog.ClientMainGetBytes));
            cmd.Parameters.Add(new SqlParameter("@bytesRelSend", _syncLog.ClientRelationsSendBytes));
            cmd.Parameters.Add(new SqlParameter("@bytesRelGet", _syncLog.ClientRelationsGetDuration));
            cmd.Parameters.Add(new SqlParameter("@durAllRel", _syncLog.AllRelationsDuration));
            cmd.Parameters.Add(new SqlParameter("@durCliIdsPut", _syncLog.ClientMainIdsUpdateDuration));
            cmd.ExecuteNonQuery();
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[8];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, 8);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

    }
}
