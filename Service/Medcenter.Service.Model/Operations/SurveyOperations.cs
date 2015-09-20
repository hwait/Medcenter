using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Types;
using ServiceStack;

namespace Medcenter.Service.Model.Operations
{
    #region SurveyPattern
    /*
     SurveyPatternSelectResponse
     SurveyPatternSaveResponse
     SurveyPatternDeleteResponse
     */


    [RequiresAnyRole("Admin", "Manager")]
    [Route("/surveypattern/{DoctorId}/{InspectionId}", "GET")]
    public class SurveyPatternSelect : IReturn<Operations.SurveyPatternSelectResponse>
    {
        public int DoctorId { get; set; }
        public int InspectionId { get; set; }
    }

    public class SurveyPatternSelectResponse : IHasResponseStatus
    {
        public SurveyPatternSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public Survey Survey { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/surveypattern/save", "POST")]
    public class SurveyPatternSave : IReturn<Operations.SurveyPatternSaveResponse>
    {
        //public int DoctorId { get; set; }
        //public int InspectionId { get; set; }
        public Survey Survey { get; set; }
    }

    public class SurveyPatternSaveResponse : IHasResponseStatus
    {
        public SurveyPatternSaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public int SurveyId { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/surveypattern/delete/{PatternId}", "GET")]
    public class SurveyPatternDelete : IReturn<Operations.SurveyPatternDeleteResponse>
    {
        public int PatternId { get; set; }
    }

    public class SurveyPatternDeleteResponse : IHasResponseStatus
    {
        public SurveyPatternDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }


    #endregion

    #region Survey
    
    #region SurveysSelect

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/surveys", "POST")]
    public class SurveySelect : IReturn<SurveySelectResponse>
    {
        public Reception Reception { get; set; }
        public int DoctorId { get; set; }
    }

    public class SurveySelectResponse : IHasResponseStatus
    {
        public SurveySelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<Survey> Surveys { get; set; }
    }

    #endregion
    
    #region SurveySave

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/survey/save", "POST")]
    public class SurveySave : IReturn<SurveySaveResponse>
    {
        //public Survey Survey { get; set; }
        public int SurveyId { get; set; }
        public ObservableCollection<Phrase> Phrases { get; set; }
    }

    public class SurveySaveResponse : IHasResponseStatus
    {
        public SurveySaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public ObservableCollection<Phrase> Phrases { get; set; }
    }

    #endregion
    
    #region SurveyDelete

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/survey/delete/{SurveyId}", "GET")]
    public class SurveyDelete : IReturn<SurveyDeleteResponse>
    {
        public int SurveyId { get; set; }
    }

    public class SurveyDeleteResponse : IHasResponseStatus
    {
        public SurveyDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    #endregion
    
    #endregion

    #region Paraphrase

    #region ParaphrasesSelect

    [RequiresAnyRole("Admin", "Manager", "Nurse")]
    [Route("/paraphrases/{InspectionId}/{DoctorId}", "GET")]
    public class ParaphraseSelect : IReturn<ParaphraseSelectResponse>
    {
        public int InspectionId { get; set; }
        public int DoctorId { get; set; }
    }

    public class ParaphraseSelectResponse : IHasResponseStatus
    {
        public ParaphraseSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<Paraphrase> Paraphrases { get; set; }
    }
    [RequiresAnyRole("Admin", "Manager", "Nurse")]
    [Route("/positions/{InspectionId}/{DoctorId}", "GET")]
    public class PositionSelect : IReturn<PositionSelectResponse>
    {
        public int InspectionId { get; set; }
        public int DoctorId { get; set; }
    }

    public class PositionSelectResponse : IHasResponseStatus
    {
        public PositionSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<Position> Positions { get; set; }
    }
    #endregion

    #region ParaphraseSave

    [RequiresAnyRole("Admin", "Manager", "Nurse")]
    [Route("/paraphrase/save", "POST")]
    public class ParaphraseSave : IReturn<ParaphraseSaveResponse>
    {
        public int DoctorId { get; set; }
        public int SurveyId { get; set; }
        public Paraphrase Paraphrase { get; set; }
    }

    public class ParaphraseSaveResponse : IHasResponseStatus
    {
        public ParaphraseSaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public int ParaphraseId { get; set; }
    }

    #endregion

    #region ParaphraseDelete

    [RequiresAnyRole("Admin", "Manager", "Nurse")]
    [Route("/paraphrase/delete/{ParaphraseId}", "GET")]
    public class ParaphraseDelete : IReturn<ParaphraseDeleteResponse>
    {
        public int ParaphraseId { get; set; }
    }

    public class ParaphraseDeleteResponse : IHasResponseStatus
    {
        public ParaphraseDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    #endregion

    #endregion
}

