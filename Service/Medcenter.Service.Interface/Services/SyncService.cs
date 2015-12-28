using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using Newtonsoft.Json;
using ServiceStack.Html;
using ServiceStack.OrmLite;

namespace Medcenter.Service.Interface.Services
{
    public class SyncService : ServiceStack.Service
    {
        public MainTablesSyncResponse Post(MainTablesSync req)
        {
            Logger.Log("MainTablesSyncResponse");
            int sid = 0;
            DateTime dt;
            List<SyncId> ids=new List<SyncId>();
            List<SyncStructure> syncStructures = new List<SyncStructure>();
            List<SyncRelationsStructure> syncRelationsStructure = new List<SyncRelationsStructure>();
            List<RemoveItem> syncRemoveItems = new List<RemoveItem>();

            #region 2.1 Get Server MT

            dt = DateTime.Now;
            try
            {
                
                syncStructures = Db.SqlList<SyncStructure>("EXEC synco_Main @Cid", new {Cid = req.Cid});
                Logger.Log("exec synco_Main", JsonConvert.SerializeObject(syncStructures));
            }
            catch (Exception e)
            {
                Logger.Log("synco_Main", e);
                throw;
            }
            var serverMainGetDuration = (DateTime.Now - dt).Milliseconds;

            #endregion

            #region 2.2 Get Server RT

            dt = DateTime.Now;
            try
            {
                Logger.Log("EXEC synco_All");
                syncRelationsStructure = Db.SqlList<SyncRelationsStructure>("EXEC synco_All @Cid", new { Cid = req.Cid });
            }
            catch (Exception e)
            {
                Logger.Log("synco_All", e);
                throw;
            }
            var serverRelationsGetDuration = (DateTime.Now - dt).Milliseconds;

            #endregion

            #region 2.3 Get Server RemoveList

            try
            {
                syncRemoveItems = Db.SqlList<RemoveItem>("EXEC synco_Del @Cid", new { Cid = req.Cid });
                Logger.Log("EXEC synco_Del");
            }
            catch (Exception e)
            {
                Logger.Log("synco_Del", e);
                throw;
            }

            #endregion

            // Here should be Collision Check, as a result of it we should get correct lists to save and to response
            if (syncStructures.Count>0&&req.SyncStructures.Count>0)
                foreach (var syncStructure in req.SyncStructures)
                {
                    if (syncStructures.Any(item => item.Sid == syncStructure.Sid && item.Tt == syncStructure.Tt))
                        syncStructures.RemoveAll(item => item.Sid == syncStructure.Sid && item.Tt == syncStructure.Tt);
                }
            // 2.4. Block SDB

            #region 2.5 Save MT to SDB

            dt = DateTime.Now;
            try
            {
                foreach (var ss in req.SyncStructures)
                {
                    switch (ss.Tt)
                    {
                        case 1:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Cities @Sid,@Cid, @P0, @P1", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1
                                }));
                            break;
                        case 28:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_UserAuth @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5, @P6, @P7, @P8, @P9, @P10, @P11, @P12, @P13, @P14, @P15",
                                new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2,
                                    P3 = ss.P3,
                                    P4 = ss.P4,
                                    P5 = ss.P5,
                                    P6 = ss.P6,
                                    P7 = ss.P7,
                                    P8 = ss.P8,
                                    P9 = ss.P9,
                                    P10 = ss.P10,
                                    P11 = ss.P11,
                                    P12 = ss.P12,
                                    P13 = ss.P13,
                                    P14 = ss.P14,
                                    P15 = ss.P15
                                }));
                            break;
                        case 26:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Surveys @Sid,@Cid, @P0, @P1, @P2", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2
                                }));
                            break;
                        case 24:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Schedules @Sid,@Cid, @P0, @P1, @P2, @P3, @P4", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2,
                                    P3 = ss.P3,
                                    P4 = ss.P4
                                }));
                            break;
                        case 23:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Receptions @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2,
                                    P3 = ss.P3,
                                    P4 = ss.P4,
                                    P5 = ss.P5
                                }));
                            break;
                        case 22:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Positions @Sid,@Cid, @P0, @P1", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1
                                }));
                            break;
                        case 21:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Phrases @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2,
                                    P3 = ss.P3,
                                    P4 = ss.P4,
                                    P5 = ss.P5
                                }));
                            break;
                        case 20:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Payments @Sid,@Cid, @P0, @P1, @P2, @P3, @P4", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2,
                                    P3 = ss.P3,
                                    P4 = ss.P4
                                }));
                            break;
                        case 19:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Patterns @Sid,@Cid, @P0, @P1, @P2, @P3", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2,
                                    P3 = ss.P3
                                }));
                            break;
                        case 17:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Patients @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5, @P6, @P7, @P8, @P9, @P10",
                                new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2,
                                    P3 = ss.P3,
                                    P4 = ss.P4,
                                    P5 = ss.P5,
                                    P6 = ss.P6,
                                    P7 = ss.P7,
                                    P8 = ss.P8,
                                    P9 = ss.P9,
                                    P10 = ss.P10
                                }));
                            break;
                        case 12:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Paraphrases @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5, @P6", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2,
                                    P3 = ss.P3,
                                    P4 = ss.P4,
                                    P5 = ss.P5,
                                    P6 = ss.P6
                                }));
                            break;
                        case 16:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Packages @Sid,@Cid, @P0, @P1, @P2, @P3", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2,
                                    P3 = ss.P3
                                }));
                            break;
                        case 11:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_PackageGroups @Sid,@Cid, @P0, @P1, @P2, @P3", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2,
                                    P3 = ss.P3
                                }));
                            break;
                        case 8:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Inspections @Sid,@Cid, @P0, @P1, @P2", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2
                                }));
                            break;
                        case 6:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Doctors @Sid,@Cid, @P0, @P1, @P2, @P3", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2,
                                    P3 = ss.P3
                                }));
                            break;
                        case 2:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Discounts @Sid,@Cid, @P0, @P1, @P2, @P3, @P4", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2,
                                    P3 = ss.P3,
                                    P4 = ss.P4
                                }));
                            break;
                        case 7:
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Headers @Sid,@Cid, @P0", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0
                                }));
                            break;
                    }
                    if (ss.Sid == 0) ids.Add(new SyncId(ss, sid));
                }
                Logger.Log("Save MT to SDB", req.SyncStructures.Count.ToString());
            }
            catch (Exception e)
            {
                Logger.Log("Save MT to SDB", e);
                throw;
            }
            var serverMainPutDuration = (DateTime.Now - dt).Milliseconds;
            #endregion
            
            #region 2.6 Update Sids in RT **** not realized yet
            foreach (var ss in req.SyncStructures)
            {
                switch (ss.Tt)
                {
                    case 1:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Cities @Sid,@Cid, @P0, @P1", new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1
                            }));
                        break;
                    case 28:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_UserAuth @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5, @P6, @P7, @P8, @P9, @P10, @P11, @P12, @P13, @P14, @P15",
                            new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1,
                                P2 = ss.P2,
                                P3 = ss.P3,
                                P4 = ss.P4,
                                P5 = ss.P5,
                                P6 = ss.P6,
                                P7 = ss.P7,
                                P8 = ss.P8,
                                P9 = ss.P9,
                                P10 = ss.P10,
                                P11 = ss.P11,
                                P12 = ss.P12,
                                P13 = ss.P13,
                                P14 = ss.P14,
                                P15 = ss.P15
                            }));
                        break;
                    case 26:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Surveys @Sid,@Cid, @P0, @P1, @P2", new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1,
                                P2 = ss.P2
                            }));
                        break;
                    case 24:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Schedules @Sid,@Cid, @P0, @P1, @P2, @P3, @P4", new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1,
                                P2 = ss.P2,
                                P3 = ss.P3,
                                P4 = ss.P4
                            }));
                        break;
                    case 23:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Receptions @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5", new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1,
                                P2 = ss.P2,
                                P3 = ss.P3,
                                P4 = ss.P4,
                                P5 = ss.P5
                            }));
                        break;
                    case 22:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Positions @Sid,@Cid, @P0, @P1", new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1
                            }));
                        break;
                    case 21:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Phrases @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5", new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1,
                                P2 = ss.P2,
                                P3 = ss.P3,
                                P4 = ss.P4,
                                P5 = ss.P5
                            }));
                        break;
                    case 20:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Payments @Sid,@Cid, @P0, @P1, @P2, @P3, @P4", new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1,
                                P2 = ss.P2,
                                P3 = ss.P3,
                                P4 = ss.P4
                            }));
                        break;
                    case 19:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Patterns @Sid,@Cid, @P0, @P1, @P2, @P3", new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1,
                                P2 = ss.P2,
                                P3 = ss.P3
                            }));
                        break;
                    case 17:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Patients @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5, @P6, @P7, @P8, @P9, @P10", new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1,
                                P2 = ss.P2,
                                P3 = ss.P3,
                                P4 = ss.P4,
                                P5 = ss.P5,
                                P6 = ss.P6,
                                P7 = ss.P7,
                                P8 = ss.P8,
                                P9 = ss.P9,
                                P10 = ss.P10
                            }));
                        break;
                    case 12:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Paraphrases @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5, @P6", new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1,
                                P2 = ss.P2,
                                P3 = ss.P3,
                                P4 = ss.P4,
                                P5 = ss.P5,
                                P6 = ss.P6
                            }));
                        break;
                    case 16:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Packages @Sid,@Cid, @P0, @P1, @P2, @P3", new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1,
                                P2 = ss.P2,
                                P3 = ss.P3
                            }));
                        break;
                    case 11:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_PackageGroups @Sid,@Cid, @P0, @P1, @P2, @P3", new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1,
                                P2 = ss.P2,
                                P3 = ss.P3
                            }));
                        break;
                    case 8:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Inspections @Sid,@Cid, @P0, @P1, @P2", new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1,
                                P2 = ss.P2
                            }));
                        break;
                    case 6:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Doctors @Sid,@Cid, @P0, @P1, @P2, @P3", new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1,
                                P2 = ss.P2,
                                P3 = ss.P3
                            }));
                        break;
                    case 2:
                        sid = GetScalar(Db.SqlList<int>(
                            "EXEC synci_Discounts @Sid,@Cid, @P0, @P1, @P2, @P3, @P4", new
                            {
                                Sid = ss.Sid,
                                Cid = req.Cid,
                                P0 = ss.P0,
                                P1 = ss.P1,
                                P2 = ss.P2,
                                P3 = ss.P3,
                                P4 = ss.P4
                            }));
                        break;
                }
                if (ss.Sid == 0) ids.Add(new SyncId(ss, sid));
            }
            #endregion

            #region 2.7 Save RT to SDB

            dt = DateTime.Now;
            try
            {
                foreach (var ss in req.SyncRelationsStructures)
                {
                    switch (ss.Tt)
                    {
                        case 25:
                            Db.SqlList<int>(
                                "EXEC synci_SurveyPhrases @Id0,@Id1", new
                                {
                                    Id0 = ss.Id0,
                                    Id1 = ss.Id1
                                });
                            break;
                        case 15:
                            Db.SqlList<int>(
                                "EXEC synci_PackagesInReception @Id0,@Id1", new
                                {
                                    Id0 = ss.Id0,
                                    Id1 = ss.Id1
                                });
                            break;
                        case 14:
                            Db.SqlList<int>(
                                "EXEC synci_PackagesInGroups @Id0,@Id1", new
                                {
                                    Id0 = ss.Id0,
                                    Id1 = ss.Id1
                                });
                            break;
                        case 13:
                            Db.SqlList<int>(
                                "EXEC synci_PackagesInDoctors @Id0,@Id1", new
                                {
                                    Id0 = ss.Id0,
                                    Id1 = ss.Id1
                                });
                            break;
                        case 10:
                            Db.SqlList<int>(
                                "EXEC synci_NursesInDoctors @Id0,@Id1", new
                                {
                                    Id0 = ss.Id0,
                                    Id1 = ss.Id1
                                });
                            break;
                        case 9:
                            Db.SqlList<int>(
                                "EXEC synci_InspectionsInPackages @Id0,@Id1", new
                                {
                                    Id0 = ss.Id0,
                                    Id1 = ss.Id1
                                });
                            break;
                        case 5:
                            Db.SqlList<int>(
                                "EXEC synci_DoctorPatterns @Id0,@Id1", new
                                {
                                    Id0 = ss.Id0,
                                    Id1 = ss.Id1
                                });
                            break;
                        case 3:
                            Db.SqlList<int>(
                                "EXEC synci_DiscountsInPackages @Id0,@Id1", new
                                {
                                    Id0 = ss.Id0,
                                    Id1 = ss.Id1
                                });
                            break;
                        case 4:
                            Db.SqlList<int>(
                                "EXEC synci_DoctorInspectionParaphrases @Id0,@Id1,@Id2", new
                                {
                                    Id0 = ss.Id0,
                                    Id1 = ss.Id1,
                                    Id2 = ss.Id2
                                });
                            break;
                        case 18:
                            Db.SqlList<int>(
                                "EXEC synci_PatternPositions @Id0,@Id1,@Cid,@Id2,@P0,@P1,@P2", new
                                {
                                    Id0 = ss.Id0,
                                    Id1 = ss.Id1,
                                    Cid = req.Cid,
                                    Id2 = ss.Id2,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2
                                });
                            break;
                    }
                }
                Logger.Log("Save RT to SDB", req.SyncRelationsStructures.Count.ToString());
            }
            catch (Exception e)
            {
                Logger.Log("Save RT to SDB", e);
                throw;
            }
            var serverRelationsPutDuration = (DateTime.Now - dt).Milliseconds;
            #endregion

            #region 2.8 SyncLog to SDB

            try
            {
                Db.SqlList<int>(
                    "EXEC synci_SyncLog @Cid,@ClientMainGetDuration,@ClientRelationsGetDuration,@ClientMainPutDuration,@ClientRelationsPutDuration,@ServerMainGetDuration," +
                    "@ServerRelationsGetDuration,@ServerMainPutDuration,@ServerRelationsPutDuration,@AllMainDuration,@ClientMainSendBytes,@ClientMainGetBytes,@ClientRelationsSendBytes," +
                    "@ClientRelationsGetBytes,@AllRelationsDuration,@ClientMainIdsUpdateDuration",
                    new
                    {
                        Cid = req.Cid,
                        ClientMainGetDuration = req.OldSyncLog.ClientMainGetDuration,
                        ClientRelationsGetDuration = req.OldSyncLog.ClientRelationsGetDuration,
                        ClientMainPutDuration = req.OldSyncLog.ClientMainPutDuration,
                        ClientRelationsPutDuration = req.OldSyncLog.ClientRelationsPutDuration,
                        ServerMainGetDuration = req.OldSyncLog.ServerMainGetDuration,
                        ServerRelationsGetDuration = req.OldSyncLog.ServerRelationsGetDuration,
                        ServerMainPutDuration = req.OldSyncLog.ServerMainPutDuration,
                        ServerRelationsPutDuration = req.OldSyncLog.ServerRelationsPutDuration,
                        AllMainDuration = req.OldSyncLog.AllMainDuration,
                        ClientMainSendBytes = req.OldSyncLog.ClientMainSendBytes,
                        ClientMainGetBytes = req.OldSyncLog.ClientMainGetBytes,
                        ClientRelationsSendBytes = req.OldSyncLog.ClientRelationsSendBytes,
                        ClientRelationsGetBytes = req.OldSyncLog.ClientRelationsGetBytes,
                        AllRelationsDuration = req.OldSyncLog.AllRelationsDuration,
                        ClientMainIdsUpdateDuration = req.OldSyncLog.ClientMainIdsUpdateDuration,
                    });
                Logger.Log("2.8 SyncLog to SDB");
            }
            catch (Exception e)
            {
                Logger.Log("2.8 SyncLog to SDB", e, "EXEC synci_SyncLog 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0");
                throw;
            }

            #endregion

            // 2.9 Deblock SDB

            #region 2.10 Response

            var resp = new MainTablesSyncResponse
            {
                Ids = ids,
                MainStructures = syncStructures,
                RelationsStructures = syncRelationsStructure,
                RemoveStructures = syncRemoveItems,
                MainGetDur = serverMainGetDuration,
                MainPutDur = serverMainPutDuration,
                RelGetDur = serverRelationsGetDuration,
                RelPutDur = serverRelationsPutDuration
            };
            //Logger.Log("resp", JsonConvert.SerializeObject(resp));
            #endregion
            
            return resp;
        }
        private string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        private int GetScalar(List<int> rows)
        {
            return (rows.Count > 0) ? rows[0] : 0;
        }
        public RelationsSyncResponse Post(RelationsSync req)
        {
            // 4.1
            #region SyncRelationsStructures

            var dt = DateTime.Now;
            foreach (var ss in req.SyncRelationsStructures)
            {
                switch (ss.Tt)
                {
                    case 25:
                        Db.SqlList<int>(
                            "EXEC synci_SurveyPhrases @Id0,@Id1", new
                            {
                                Id0 = ss.Id0,
                                Id1 = ss.Id1
                            });
                        break;
                    case 15:
                        Db.SqlList<int>(
                            "EXEC synci_PackagesInReception @Id0,@Id1", new
                            {
                                Id0 = ss.Id0,
                                Id1 = ss.Id1
                            });
                        break;
                    case 14:
                        Db.SqlList<int>(
                            "EXEC synci_PackagesInGroups @Id0,@Id1", new
                            {
                                Id0 = ss.Id0,
                                Id1 = ss.Id1
                            });
                        break;
                    case 13:
                        Db.SqlList<int>(
                            "EXEC synci_PackagesInDoctors @Id0,@Id1", new
                            {
                                Id0 = ss.Id0,
                                Id1 = ss.Id1
                            });
                        break;
                    case 10:
                        Db.SqlList<int>(
                            "EXEC synci_NursesInDoctors @Id0,@Id1", new
                            {
                                Id0 = ss.Id0,
                                Id1 = ss.Id1
                            });
                        break;
                    case 9:
                        Db.SqlList<int>(
                            "EXEC synci_InspectionsInPackages @Id0,@Id1", new
                            {
                                Id0 = ss.Id0,
                                Id1 = ss.Id1
                            });
                        break;
                    case 5:
                        Db.SqlList<int>(
                            "EXEC synci_DoctorPatterns @Id0,@Id1", new
                            {
                                Id0 = ss.Id0,
                                Id1 = ss.Id1
                            });
                        break;
                    case 3:
                        Db.SqlList<int>(
                            "EXEC synci_DiscountsInPackages @Id0,@Id1", new
                            {
                                Id0 = ss.Id0,
                                Id1 = ss.Id1
                            });
                        break;
                    case 4:
                        Db.SqlList<int>(
                            "EXEC synci_DoctorInspectionParaphrases @Id0,@Id1,@Id2", new
                            {
                                Id0 = ss.Id0,
                                Id1 = ss.Id1,
                                Id2 = ss.Id2
                            });
                        break;
                    case 18:
                        Db.SqlList<int>(
                            "EXEC synci_PatternPositions @Id0,@Id1,@Cid,@Id2,@P0,@P1,@P2", new
                            {
                                Id0 = ss.Id0,
                                Id1 = ss.Id1,
                                Cid = req.Cid,
                                Id2 = ss.Id2,
                                P0 = ss.P0,
                                P1 = ss.P1,
                                P2 = ss.P2
                            });
                        break;
                }
            }
            var serverRelationsPutDuration = (DateTime.Now - dt).Milliseconds;
            #endregion

            
            
            dt = DateTime.Now;

            // 4.2
            var syncRelationsStructures = Db.SqlList<SyncRelationsStructure>("EXEC synco_All @Cid, @rv", new { rv = req.Srv, Cid = req.Cid });
            var serverRelationsGetDuration = (DateTime.Now - dt).Milliseconds;

            // 4.3
            return new RelationsSyncResponse { SyncRelationsStructures = syncRelationsStructures, DurGet = serverRelationsGetDuration, DurPut = serverRelationsPutDuration };
        }
    }
}
