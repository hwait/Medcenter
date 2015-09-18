using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
				survey.Phrases = new ObservableCollection<Phrase>(Db.SqlList<Phrase>("EXEC sp_PositionsAsPhrases_Select @PatternId", new { PatternId = survey.Id }));
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
						DoctorId = req.Survey.DoctorId,
						InspectionId = req.Survey.InspectionId,
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
						"EXEC sp_Survey_SelectOrInsert @ReceptionId, @InspectionId, @DoctorId", new
						{
							ReceptionId = req.Reception.Id,
							InspectionId = inspectionId,
							DoctorId = req.DoctorId
						});
					//Only single value is expected
					if (rows.Count > 0)
					{
						surveys.Add(SurveyFillValues(rows[0], req.DoctorId, inspectionId));
					}
				}
			}
			
			return new SurveySelectResponse { Surveys = surveys };
		}

		private Survey SurveyFillValues(Survey survey, int did, int iid)
		{
			

			survey.Phrases = GetPhrases(survey.Id);
			//if (rows[0].Phrases.Count == 0)
			//    rows[0].Phrases.Add(new Phrase(0));
			survey.ParaphrasesBase = Db.SqlList<Paraphrase>("EXEC sp_Paraphrases_Select @DoctorId, @InspectionId", new { DoctorId = did, InspectionId = iid });
			survey.DoctorId = did;
			survey.InspectionId = iid;
			return survey;
		}

		private ObservableCollection<Phrase> GetPhrases(int id)
		{
			var patternPhrases = Db.SqlList<Phrase>("EXEC sp_PatternPhrases_Select @SurveyId", new { SurveyId = id });
			var phrases = Db.SqlList<Phrase>("EXEC sp_Phrases_Select @SurveyId", new { SurveyId = id });
			var result = new ObservableCollection<Phrase>();
			var isFound = false;
			foreach (var patternPhrase in patternPhrases)
			{
				isFound = false;
				foreach (var phrase in phrases)
				{
					if (phrase.PositionId == patternPhrase.PositionId)
					{
						isFound = true;
						phrase.PositionName = patternPhrase.PositionName;
						phrase.Type = patternPhrase.Type;
						phrase.ShowOrder = patternPhrase.ShowOrder;
						phrase.DecorationType = patternPhrase.DecorationType;
						result.Add(phrase);
					}
				}
				if (!isFound)
					result.Add(patternPhrase);
			}
			return new ObservableCollection<Phrase>(result.OrderBy(p => p.PositionId));
		}
		public SurveySaveResponse Post(SurveySave req)
		{
			ResultMessage message;
			// Survey ALWAYS exists so we're saving PHRASES ONLY
			try
			{
				foreach (var phrase in req.Phrases)
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
								SurveyId=req.SurveyId,
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
				Phrases=GetPhrases(req.SurveyId)
			};
		}
		#endregion
		
		#region Paraphrase
		public ParaphraseSelectResponse Get(ParaphraseSelect req)
		{
			var rows = Db.SqlList<Paraphrase>(
				"EXEC sp_Paraphrase_Select @InspectionId, @DoctorId", new
				{
					InspectionId = req.InspectionId,
					DoctorId = req.DoctorId
				});

			return new ParaphraseSelectResponse { Paraphrases = rows };
		}

		public ParaphraseSaveResponse Post(ParaphraseSave req)
		{
			ResultMessage message;
			int id = req.Paraphrase.Id;
			try
			{
				if (id == 0) //New Paraphrase
				{
					id = Db.Single<int>("EXEC sp_Paraphrases_Insert @DoctorId, @SurveyId, @Text, @PositionId, @V1, @V2, @V3, @PresetId, @ShowOrder", new
					{
						DoctorId=req.DoctorId,
						SurveyId = req.SurveyId,
						Text = req.Paraphrase.Text,
						PositionId = req.Paraphrase.PositionId,
						V1 = req.Paraphrase.V1,
						V2 = req.Paraphrase.V2,
						V3 = req.Paraphrase.V3,
						PresetId = req.Paraphrase.PresetId,
						ShowOrder = req.Paraphrase.ShowOrder
					});
				}
				else //Paraphrase Exists
				{
					Db.Single<int>("EXEC sp_Paraphrases_Update @DoctorId, @InspectionId, @Text, @PositionId, @V1, @V2, @V3, @PresetId, @ShowOrder", new
					{
						Id = req.Paraphrase.Id,
						Text = req.Paraphrase.Text,
						PositionId = req.Paraphrase.PositionId,
						V1 = req.Paraphrase.V1,
						V2 = req.Paraphrase.V2,
						V3 = req.Paraphrase.V3,
						PresetId = req.Paraphrase.PresetId,
						ShowOrder = req.Paraphrase.ShowOrder
					});
				}
			}
			catch (Exception e)
			{
				Logger.Log("ParaphraseSaveResponse.Saving", e);
				throw;
			}

			return new ParaphraseSaveResponse
			{
				ParaphraseId = id,
			};
		}
		public ParaphraseDeleteResponse Get(ParaphraseDelete req)
		{
			ResultMessage message;
			try
			{
				Db.ExecuteNonQuery("EXEC sp_Paraphrases_Delete @Id", new
				{
					Id = req.ParaphraseId
				});
			}
			catch (Exception e)
			{
				Logger.Log("ParaphraseDeleteResponse.Saving", e);
				throw;
			}

			return new ParaphraseDeleteResponse
			{
			};
		}
		#endregion
	}
}
