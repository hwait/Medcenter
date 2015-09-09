﻿using System;
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
    public class SurveyService : ServiceStack.Service
    {
        
        #region SurveyPattern
        /*
             SurveyPatternSelectResponse
             SurveyPatternSaveResponse
             SurveyPatternDeleteResponse
        */
        public SurveyPatternSelectResponse Get(SurveyPatternSelect req)
        {
            var rows = Db.SqlList<Survey>("EXEC sp_PatternAsSurvey_Select @DoctorId, @InspectionId", new { DoctorId = req.DoctorId, InspectionId = req.InspectionId}); //Only single value is expected
            Survey survey = rows.Count > 0 ? rows[0] : null;
            if (rows.Count > 0)
            {
                survey.Phrases = Db.SqlList<Phrase>("EXEC sp_PositionsAsPhrases_Select @PatternId", new { PatternId = survey.Id });
                if (survey.Phrases.Count == 0)
                    survey.Phrases.Add(new Phrase(0));
            }
            return new SurveyPatternSelectResponse { Survey = survey };
        }

        public SurveyPatternSaveResponse Post(SurveyPatternSave req)
        {
            int id = 0;
            ResultMessage message;
            if (req.Survey.Id > 0) // Survey exists so we're saving 
            {
                id = req.Survey.Id;
                try
                {
                    Db.Single<int>("EXEC sp_PatternAsSurvey_UpdateHeader @PatternId, @Header", new
                    {
                        PatternId = req.Survey.Id,
                        Header = req.Survey.Header
                    });
                    foreach (var phrase in req.Survey.Phrases)
                    {
                        if (phrase.Id == 0) phrase.Status = 2;
                        switch (phrase.Status) // 1 - Changed, 2 - New, 3 - To Delete, 4 - Cut, 5 - Copied from another Pattern
                        {
                            case 1:
                            case 4:
                                Db.Single<int>("EXEC sp_PositionAsPhrase_Update @PatternId,@PositionId, @Text,@PositionName,@ShowOrder,@DecorationType,@Type", new
                                {
                                    PatternId = req.Survey.Id,
                                    PositionId = phrase.Id,
                                    Text = phrase.Text,
                                    PositionName = phrase.PositionName,
                                    ShowOrder = phrase.ShowOrder,
                                    DecorationType = phrase.DecorationType,
                                    Type = phrase.Type
                                });
                                break;
                            case 2:
                                phrase.Id = Db.Single<int>("EXEC sp_PositionAsPhrase_Insert @PatternId, @Text,@PositionName,@ShowOrder,@DecorationType,@Type", new
                                {
                                    PatternId = req.Survey.Id,
                                    Text = phrase.Text,
                                    PositionName = phrase.PositionName,
                                    ShowOrder = phrase.ShowOrder,
                                    DecorationType = phrase.DecorationType,
                                    Type = phrase.Type
                                });
                                break;
                            case 3:
                                Db.Single<int>("EXEC sp_PatternPosition_Delete @PatternId, @PositionId", new
                                {
                                    PatternId = req.Survey.Id,
                                    PositionId = phrase.Id
                                });
                                break;
                            case 5:
                                Db.Single<int>("EXEC sp_PositionAsPhrase_AddToPattern @PatternId, @PositionId, @Text,@ShowOrder,@DecorationType", new
                                {
                                    PatternId = req.Survey.Id,
                                    PositionId = phrase.Id,
                                    Text = phrase.Text,
                                    ShowOrder = phrase.ShowOrder,
                                    DecorationType = phrase.DecorationType
                                });
                                break;
                        }
                    }
                    message = new ResultMessage(0, "Сохранение исследования", OperationResults.SurveyPatternSave);
                    //Logger.Log("SurveyPatternSaveResponse.Saving");
                }
                catch (Exception e)
                {
                    message = new ResultMessage(2, e.Source, OperationErrors.SurveyPatternSave);
                    Logger.Log("SurveyPatternSaveResponse.Saving", e);
                    throw;
                }
            }
            else //New Survey
            {
                try
                {
                    id=Db.Single<int>("EXEC sp_PatternAsSurvey_Insert @DoctorId,@InspectionId, @Header", new
                    {
                        DoctorId = req.DoctorId,
                        InspectionId = req.InspectionId,
                        Header = req.Survey.Header
                    });
                    foreach (var phrase in req.Survey.Phrases)
                    {
                        if (phrase.Id == 0) phrase.Status = 2;
                        switch (phrase.Status) // 2 - New, 5 - Copied from another Pattern
                        {
                            case 2:
                                Db.Single<int>("EXEC sp_PositionAsPhrase_Insert @PatternId, @Text,@PositionName,@ShowOrder,@DecorationType,@Type", new
                                {
                                    PatternId = id,
                                    Text = phrase.Text,
                                    PositionName = phrase.PositionName,
                                    ShowOrder = phrase.ShowOrder,
                                    DecorationType = phrase.DecorationType,
                                    Type = phrase.Type
                                });
                                break;
                            case 5:
                                Db.Single<int>("EXEC sp_PositionAsPhrase_AddToPattern @PatternId, @PositionId, @Text,@ShowOrder,@DecorationType", new
                                {
                                    PatternId = id,
                                    PositionId = phrase.Id,
                                    Text = phrase.Text,
                                    ShowOrder = phrase.ShowOrder,
                                    DecorationType = phrase.DecorationType
                                });
                                break;
                        }
                    }
                    message = new ResultMessage(0, "Сохранение исследования", OperationResults.SurveyPatternSave);
                    //Logger.Log("SurveyPatternSaveResponse.Saving");
                }
                catch (Exception e)
                {
                    message = new ResultMessage(2, e.Source, OperationErrors.SurveyPatternSave);
                    Logger.Log("SurveyPatternSaveResponse.New", e);
                    throw;
                }
            }

            return new SurveyPatternSaveResponse
            {
                SurveyId = id,
                Message = message
            };
        }

        public SurveyPatternDeleteResponse Get(SurveyPatternDelete req)
        {
            ResultMessage message;
            try
            {
                var res = Db.SqlList<int>("EXEC sp_Pattern_Delete @PatternId", new
                {
                    PatternId = req.PatternId,
                });
                message = new ResultMessage(0, "Бланк:", OperationResults.SurveyPatternDelete);
            }
            catch (Exception e)
            {
                message = new ResultMessage(2, e.Source, OperationErrors.SurveyPatternDelete);
                Logger.Log("SurveyPatternDeleteResponse", e);
                throw;
            }
            return new SurveyPatternDeleteResponse { Message = message };
        }

        #endregion

        #region Survey
        public SurveySelectResponse Post(SurveySelect req)
        {
            List<Survey> surveys=new List<Survey>();
            foreach (var package in req.Reception.Packages)
            {
                foreach (var inspectionId in package.InspectionIds)
                {
                    var rows = Db.SqlList<Survey>(
                        "EXEC sp_Survey_SelectOrInsert @ReceptionId, @DoctorId, @InspectionId", new
                        {
                            ReceptionId = req.Reception.Id,
                            DoctorId = req.DoctorId,
                            InspectionId = inspectionId
                        });
                    //Only single value is expected
                    if (rows.Count > 0)
                    {
                        rows[0].Phrases = Db.SqlList<Phrase>("EXEC sp_Phrases_Select @SurveyId", new { SurveyId = rows[0].Id });
                        if (rows[0].Phrases.Count == 0)
                            rows[0].Phrases.Add(new Phrase(0));
                        surveys.Add(rows[0]);
                    }
                }
            }
            
            return new SurveySelectResponse { Surveys = surveys };
        }

        public SurveySaveResponse Post(SurveySave req)
        {
            int id = 0;
            ResultMessage message;
            // Survey ALWAYS exists so we're saving PHRASES ONLY
            
            id = req.Survey.Id;
            try
            {
                foreach (var phrase in req.Survey.Phrases)
                {
                    if (phrase.Id == 0) phrase.Status = 2;
                    switch (phrase.Status) // 1 - Changed, 2 - New, 3 - To Delete, 4 - Cut
                    {
                        case 1:
                            Db.Single<int>("EXEC sp_Phrases_Update @Id, @Text, @PositionId, @V1, @V2, @V3, @ParaphraseId", new
                            {
                                Id = phrase.Id,
                                Text = phrase.Text,
                                PositionId = phrase.PositionId,
                                V1 = phrase.V1,
                                V2 = phrase.V2,
                                V3 = phrase.V3,
                                ParaphraseId = phrase.ParaphraseId
                            });
                            break;
                        case 2:
                            phrase.Id = Db.Single<int>("EXEC sp_Phrases_Insert @SurveyId, @Text, @PositionId, @V1, @V2, @V3, @ParaphraseId", new
                            {
                                SurveyId=req.Survey.Id,
                                Text = phrase.Text,
                                PositionId = phrase.PositionId,
                                V1 = phrase.V1,
                                V2 = phrase.V2,
                                V3 = phrase.V3,
                                ParaphraseId = phrase.ParaphraseId
                            });
                            break;
                        case 3:
                            Db.Single<int>("EXEC sp_Phrases_Delete @Id", new
                            {
                                Id = phrase.Id
                            });
                            break;
                    }
                }
                //Logger.Log("SurveySaveResponse.Saving");
            }
            catch (Exception e)
            {
                Logger.Log("SurveySaveResponse.Saving", e);
                throw;
            }
           
            return new SurveySaveResponse
            {
                SurveyId = id,
            };
        }
        #endregion
    }
}