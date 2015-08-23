using System;
using System.Collections.Generic;
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
        public int DoctorId { get; set; }
        public int InspectionId { get; set; }
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
    [Route("/surveypattern/delete", "POST")]
    public class SurveyPatternDelete : IReturn<Operations.SurveyPatternDeleteResponse>
    {
        public Survey Survey { get; set; }
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
}

