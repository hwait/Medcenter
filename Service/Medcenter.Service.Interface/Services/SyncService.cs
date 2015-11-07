using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using ServiceStack.OrmLite;

namespace Medcenter.Service.Interface.Services
{
    public class SyncService : ServiceStack.Service
    {
        public MainTablesSyncResponse Post(MainTablesSync req)
        {
            List<SyncId> ids=new List<SyncId>();
            int sid=0;
            foreach (var ss in req.SyncStructures)
            {
                switch (ss.TableType)
                {
                    case 1:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Cities @Sid,@Cid, @P0, @P1", new
                            {                        
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1
                            }));
                        break;
                    case 28:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_UserAuth @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5, @P6, @P7, @P8, @P9, @P10, @P11, @P12, @P13, @P14, @P15", new {
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1,P2 = ss.P2,P3 = ss.P3,P4 = ss.P4,P5 = ss.P5,P6 = ss.P6,P7 = ss.P7,P8 = ss.P8,P9 = ss.P9,P10 = ss.P10,P11 = ss.P11,P12 = ss.P12,P13 = ss.P13,P14 = ss.P14,P15 = ss.P15
                            }));
                        break;
                    case 26:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Surveys @Sid,@Cid, @P0, @P1, @P2", new {
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1,P2 = ss.P2
                            }));
                        break;
                    case 24:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Schedules @Sid,@Cid, @P0, @P1, @P2, @P3, @P4", new
                            {
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1,P2 = ss.P2,P3 = ss.P3,P4 = ss.P4
                            }));
                        break;
                    case 23:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Receptions @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5", new
                            {
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1,P2 = ss.P2,P3 = ss.P3,P4 = ss.P4,P5 = ss.P5
                            }));
                        break;
                    case 22:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Positions @Sid,@Cid, @P0, @P1", new
                            {
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1
                            }));
                        break;
                    case 21:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Phrases @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5", new
                            {
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1,P2 = ss.P2,P3 = ss.P3,P4 = ss.P4,P5 = ss.P5
                            }));
                        break;
                    case 20:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Payments @Sid,@Cid, @P0, @P1, @P2, @P3, @P4", new
                            {
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1,P2 = ss.P2,P3 = ss.P3,P4 = ss.P4
                            }));
                        break;
                    case 19:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Patterns @Sid,@Cid, @P0, @P1, @P2, @P3", new
                            {
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1,P2 = ss.P2,P3 = ss.P3
                            }));
                        break;
                    case 17:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Patients @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5, @P6, @P7, @P8, @P9, @P10", new
                            {
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1,P2 = ss.P2,P3 = ss.P3,P4 = ss.P4,P5 = ss.P5,P6 = ss.P6,P7 = ss.P7,P8 = ss.P8,P9 = ss.P9,P10 = ss.P10
                            }));
                        break;
                    case 12:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Paraphrases @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5, @P6", new
                            {
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1,P2 = ss.P2,P3 = ss.P3,P4 = ss.P4,P5 = ss.P5,P6 = ss.P6
                            }));
                        break;
                    case 16:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Packages @Sid,@Cid, @P0, @P1, @P2, @P3", new
                            {
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1,P2 = ss.P2,P3 = ss.P3
                            }));
                        break;
                    case 11:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_PackageGroups @Sid,@Cid, @P0, @P1, @P2, @P3", new
                            {
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1,P2 = ss.P2,P3 = ss.P3
                            }));
                        break;
                    case 8:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Inspections @Sid,@Cid, @P0, @P1, @P2", new
                            {
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1,P2 = ss.P2
                            }));
                        break;
                    case 6:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Doctors @Sid,@Cid, @P0, @P1, @P2, @P3", new
                            {
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1,P2 = ss.P2,P3 = ss.P3
                            }));
                        break;
                    case 2:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Discounts @Sid,@Cid, @P0, @P1, @P2, @P3, @P4", new
                            {
                                Sid = ss.Sid,Cid = req.Cid,P0 = ss.P0,P1 = ss.P1,P2 = ss.P2,P3 = ss.P3,P4 = ss.P4
                            }));
                        break;
                }
                if (ss.Sid == 0) ids.Add(new SyncId(ss, sid));
            }

            return new MainTablesSyncResponse { Ids = ids };
        }

        private int GetScalar(List<int> rows)
        {
            return (rows.Count > 0) ? rows[0] : 0;
        }
        public RelationsSyncResponse Post(RelationsSync req)
        {
            List<SyncId> SyncStructuresids=new List<SyncId>();
            int sid=0;
            foreach (var ss in req.SyncStructures)
            {
                switch (ss.TableType)
                {
                    case 25:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_SurveyPhrases @Id0,@Id1", new
                            {                        
                                Id0 = ss.Id0,Id1 = ss.Id1
                            }));
                        break;
                    case 15:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_PackagesInReception @Id0,@Id1", new
                            {                        
                                Id0 = ss.Id0,Id1 = ss.Id1
                            }));
                        break;
                     case 14:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_PackagesInGroups @Id0,@Id1", new
                            {                        
                                Id0 = ss.Id0,Id1 = ss.Id1
                            }));
                        break;
                    case 13:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_PackagesInDoctors @Id0,@Id1", new
                            {                        
                                Id0 = ss.Id0,Id1 = ss.Id1
                            }));
                        break;
                    case 10:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_NursesInDoctors @Id0,@Id1", new
                            {                        
                                Id0 = ss.Id0,Id1 = ss.Id1
                            }));
                        break;
                    case 9:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_InspectionsInPackages @Id0,@Id1", new
                            {                        
                                Id0 = ss.Id0,Id1 = ss.Id1
                            }));
                        break;
                     case 5:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_DoctorPatterns @Id0,@Id1", new
                            {                        
                                Id0 = ss.Id0,Id1 = ss.Id1
                            }));
                        break;
                    case 3:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_DiscountsInPackages @Id0,@Id1", new
                            {                        
                                Id0 = ss.Id0,Id1 = ss.Id1
                            }));
                        break;
                        case 4:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_DoctorInspectionParaphrases @Id0,@Id1,@Id2", new
                            {                        
                                Id0 = ss.Id0,Id1 = ss.Id1,Id2 = ss.Id2
                            }));
                        break;
                    case 18:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_PatternPositions @Id0,@Id1,@Cid,@Id2,@P0,@P1,@P2", new
                            {
                                Id0 = ss.Id0,Id1 = ss.Id1,Cid = req.Cid,Id2 = ss.Id2,P0 = ss.P0,P1 = ss.P1,P2 = ss.P2
                            }));
                        break;
                }
            }
            var syncStructures = Db.SqlList<SyncStructure>("EXEC synco_Main @Cid, @rv", new { Cid = req.Cid, rv = req.Rv });
            var syncRelationsStructures = Db.SqlList<SyncRelationsStructure>("EXEC synco_All @Cid, @rv", new { Cid = req.Cid, rv = req.Rv });
            return new RelationsSyncResponse { SyncStructures = syncStructures, SyncRelationsStructures = syncRelationsStructures };
        }
    }
}
