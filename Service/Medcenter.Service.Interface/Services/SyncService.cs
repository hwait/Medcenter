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
            int sid0=0, sid1=0, sid2=0;

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
                            sid0 = (ss.P1 != "0") ? int.Parse(ss.P1) : GetSidFromIds(ids, 19, ss.P3);
                            sid1 = (ss.P2 != "0") ? int.Parse(ss.P2) : GetSidFromIds(ids, 23, ss.P4);
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Surveys @Sid,@Cid, @P0, @P1, @P2", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,     // Dt
                                    P1 = sid0,     // PatternId
                                    P2 = sid1      // ReceptionId
                                }));
                            break;
                        case 24:
                            sid0 = (ss.P0 != "0") ? int.Parse(ss.P0) : GetSidFromIds(ids, 6, ss.P5);
                            sid1 = (ss.P4 != "0") ? int.Parse(ss.P4) : GetSidFromIds(ids, 28, ss.P6);
                            Logger.Log(String.Format("synci_Schedules @Sid={0},@Cid={1}, @P0={2} (id={7}, sid={9}), @P1={3}, @P2={4}, @P3={5}, @P4={6} (id={8}, sid={10}): ", ss.Sid, req.Cid, sid0, ss.P1, ss.P2, ss.P3, sid1, ss.P5, ss.P6, ss.P0, ss.P4), JsonConvert.SerializeObject(ids));
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Schedules @Sid,@Cid, @P0, @P1, @P2, @P3, @P4", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = sid0,     // DoctorId	
                                    P1 = ss.P1,     // CabinetId	
                                    P2 = ss.P2,     // TimeStart	
                                    P3 = ss.P3,     // TimeEnd	
                                    P4 = sid1      // NurseId	
                                }));
                            break;
                        case 23: // !!!!!!!!!!!!!! RefererId hasn't complete!!!!!!!!!!!!!!!!!
                            sid0 = (ss.P0 != "0") ? int.Parse(ss.P0) : GetSidFromIds(ids, 15, ss.P6);
                            sid1 = (ss.P1 != "0") ? int.Parse(ss.P1) : GetSidFromIds(ids, 24, ss.P7);
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Receptions @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = sid0,     // PatientId	
                                    P1 = sid1,     // ScheduleId
                                    P2 = ss.P2,     // Start	
                                    P3 = ss.P3,     // Duration	
                                    P4 = ss.P4,     // Status	
                                    P5 = ss.P5      // RefererId	
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
                            sid0 = (ss.P0 != "0") ? int.Parse(ss.P0) : GetSidFromIds(ids, 16, ss.P6);
                            sid1 = (ss.P5 != "0") ? int.Parse(ss.P5) : GetSidFromIds(ids, 22, ss.P7);
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Phrases @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = sid0,     // ParaphraseId	
                                    P1 = ss.P1,     // Text	
                                    P2 = ss.P2,     // V1	
                                    P3 = ss.P3,     // V2	
                                    P4 = ss.P4,     // V3		
                                    P5 = sid1      // PositionId	
                                }));
                            break;
                        case 20:
                            sid0 = (ss.P2 != "0") ? int.Parse(ss.P2) : GetSidFromIds(ids, 23, ss.P5);
                            sid1 = (ss.P3 != "0") ? int.Parse(ss.P3) : GetSidFromIds(ids, 2, ss.P6);
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Payments @Sid,@Cid, @P0, @P1, @P2, @P3, @P4", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,     // Cost	
                                    P1 = ss.P1,     // Dt	
                                    P2 = sid0,     // ReceptionId	
                                    P3 = sid1,     // DiscountId	
                                    P4 = ss.P4      // FinalCost		
                                }));
                            break;
                        case 19:
                            sid0 = (ss.P0 != "0") ? int.Parse(ss.P0) : GetSidFromIds(ids, 8, ss.P4);
                            sid1 = (ss.P1 != "0") ? int.Parse(ss.P1) : GetSidFromIds(ids, 7, ss.P5);
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Patterns @Sid,@Cid, @P0, @P1, @P2, @P3", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = sid0,     // InspectionId	
                                    P1 = sid1,     // HeaderId	
                                    P2 = ss.P2,     // Picture	
                                    P3 = ss.P3      // PictureType	
                                }));
                            break;
                        case 17:
                            sid0 = (ss.P10 != "0") ? int.Parse(ss.P10) : GetSidFromIds(ids, 1, ss.P11);
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Patients @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5, @P6, @P7, @P8, @P9, @P10",
                                new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,     // Surname		
                                    P1 = ss.P1,     // FirstName	
                                    P2 = ss.P2,     // SecondName	
                                    P3 = ss.P3,     // BirthDate	
                                    P4 = ss.P4,     // Gender		
                                    P5 = ss.P5,     // Address		
                                    P6 = ss.P6,     // PhoneNumber	
                                    P7 = ss.P7,     // MobileCode	
                                    P8 = ss.P8,     // MobileNumber	
                                    P9 = ss.P9,     // Email		
                                    P10 = sid0    // CityId
                                }));
                            break;
                        case 12:
                            sid0 = (ss.P1 != "0") ? int.Parse(ss.P1) : GetSidFromIds(ids, 22, ss.P7);
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_Paraphrases @Sid,@Cid, @P0, @P1, @P2, @P3, @P4, @P5, @P6", new
                                {
                                    Sid = ss.Sid,
                                    Cid = req.Cid,
                                    P0 = ss.P0,     // Text		
                                    P1 = sid0,     // PositionId
                                    P2 = ss.P2,     // V1	
                                    P3 = ss.P3,     // V2	
                                    P4 = ss.P4,     // V3		
                                    P5 = ss.P5,     // PresetId	
                                    P6 = ss.P6      // ShowOrder	
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
                Logger.Log("Save MT to SDB, Ids: ", JsonConvert.SerializeObject(ids));
            }
            catch (Exception e)
            {
                Logger.Log("Save MT to SDB", e);
                throw;
            }
            var serverMainPutDuration = (DateTime.Now - dt).Milliseconds;
            #endregion
            
            #region 2.6 Update Sids in RT  and save RT

            
            try
            {
                foreach (var ss in req.SyncRelationsStructures)
                {
                    sid0=0;
                    sid1=0;
                    sid2=0;
                    switch (ss.Tt)
                    {
                        case 3: //3	DiscountsInPackages (2, 12)
                            sid0 = (ss.Sid0 == 0) ? GetSidFromIds(ids, 2, ss.Id0) : ss.Sid0;
                            sid1 = (ss.Sid1 == 0) ? GetSidFromIds(ids, 12, ss.Id1) : ss.Sid1;
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_DiscountsInPackages @Id0, @Id1", new
                                {
                                    Id0 = sid0,
                                    Id1 = sid1
                                }));
                            
                            break;
                        case 4: //	DoctorInspectionParaphrases (6, 8, 16)
                            sid0 = (ss.Sid0 == 0) ? GetSidFromIds(ids, 6, ss.Id0) : ss.Sid0;
                            sid1 = (ss.Sid1 == 0) ? GetSidFromIds(ids, 8, ss.Id1) : ss.Sid1;
                            sid2 = (ss.Sid2 == 0) ? GetSidFromIds(ids, 16, ss.Id2) : ss.Sid2;
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_DoctorInspectionParaphrases @Id0, @Id1, @Id2",
                                new
                                {
                                    Id0 = sid0,
                                    Id1 = sid1,
                                    Id2 = sid2
                                }));
                            break;
                        case 5: //5	DoctorPatterns (6, 19)
                            sid0 = (ss.Sid0 == 0) ? GetSidFromIds(ids, 6, ss.Id0) : ss.Sid0;
                            sid1 = (ss.Sid1 == 0) ? GetSidFromIds(ids, 19, ss.Id1) : ss.Sid1;
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_DoctorPatterns @Id0, @Id1", new
                                {
                                    Id0 = sid0,
                                    Id1 = sid1
                                }));
                            break;
                        case 9: //9	InspectionsInPackages (8, 12)
                            sid0 = (ss.Sid0 == 0) ? GetSidFromIds(ids, 8, ss.Id0) : ss.Sid0;
                            sid1 = (ss.Sid1 == 0) ? GetSidFromIds(ids, 12, ss.Id1) : ss.Sid1;
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_InspectionsInPackages @Id0, @Id1", new
                                {
                                    Id0 = sid0,
                                    Id1 = sid1
                                }));
                            break;
                        case 10: //10	NursesInDoctors (28, 6)
                            sid0 = (ss.Sid0 == 0) ? GetSidFromIds(ids, 28, ss.Id0) : ss.Sid0;
                            sid1 = (ss.Sid1 == 0) ? GetSidFromIds(ids, 6, ss.Id1) : ss.Sid1;
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_NursesInDoctors @Id0, @Id1", new
                                {
                                    Id0 = sid0,
                                    Id1 = sid1
                                }));
                            break;
                        case 13: //13	PackagesInDoctors (12, 6)
                            sid0 = (ss.Sid0 == 0) ? GetSidFromIds(ids, 12, ss.Id0) : ss.Sid0;
                            sid1 = (ss.Sid1 == 0) ? GetSidFromIds(ids, 6, ss.Id1) : ss.Sid1;
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_PackagesInDoctors @Id0, @Id1", new
                                {
                                    Id0 = sid0,
                                    Id1 = sid1
                                }));
                            break;
                        case 14: //14	PackagesInGroups (12, 11)
                            sid0 = (ss.Sid0 == 0) ? GetSidFromIds(ids, 12, ss.Id0) : ss.Sid0;
                            sid1 = (ss.Sid1 == 0) ? GetSidFromIds(ids, 11, ss.Id1) : ss.Sid1;
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_PackagesInGroups @Id0, @Id1", new
                                {
                                    Id0 = sid0,
                                    Id1 = sid1
                                }));
                            break;
                        case 15: //15	PackagesInReception (12, 23)
                            sid0 = (ss.Sid0 == 0) ? GetSidFromIds(ids, 12, ss.Id0) : ss.Sid0;
                            sid1 = (ss.Sid1 == 0) ? GetSidFromIds(ids, 23, ss.Id1) : ss.Sid1;
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_PackagesInReception @Id0, @Id1", new
                                {
                                    Id0 = sid0,
                                    Id1 = sid1
                                }));
                            break;
                        case 18: //18	PatternPositions (19, 22)
                            sid0 = (ss.Sid0 == 0) ? GetSidFromIds(ids, 19, ss.Id0) : ss.Sid0;
                            sid1 = (ss.Sid1 == 0) ? GetSidFromIds(ids, 22, ss.Id1) : ss.Sid1;
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_PatternPositions @Id0, @Id1, @Id2, @P0, @P1, @P2", new
                                {
                                    Id0 = sid0,
                                    Id1 = sid1,
                                    Id2 = ss.Id2,
                                    P0 = ss.P0,
                                    P1 = ss.P1,
                                    P2 = ss.P2
                                }));
                            break;
                        case 25: //25	SurveyPhrases (26, 21)
                            sid0 = (ss.Sid0 == 0) ? GetSidFromIds(ids, 26, ss.Id0) : ss.Sid0;
                            sid1 = (ss.Sid1 == 0) ? GetSidFromIds(ids, 21, ss.Id1) : ss.Sid1;
                            sid = GetScalar(Db.SqlList<int>(
                                "EXEC synci_SurveyPhrases @Id0, @Id1", new
                                {
                                    Id0 = sid0,
                                    Id1 = sid1
                                }));
                            break;
                    }
                    Logger.Log(String.Format(" TT:{0}, sid0={1}, sid1={2}, sid2={3}, ",ss.Tt, sid0, sid1,sid2));
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
                Logger.Log("2.8 SyncLog to SDB", e);
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

        private int GetSidFromIds(List<SyncId> ids, int tt, string id0Txt)
        {
            int id0 = int.Parse(id0Txt);
            var syncId = ids.FirstOrDefault(x => x.Id == id0 && x.Tt == tt);

            return syncId == null ? 0 : syncId.Sid;
        }

        private int GetSidFromIds(List<SyncId> ids, int tt, int id0)
        {
            var syncId = ids.FirstOrDefault(x => x.Id == id0 && x.Tt == tt);

            return syncId == null ? 0 : syncId.Sid;
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
