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
    #region Doctor

    [Authenticate]
    [Route("/doctors", "GET")]
    public class DoctorsSelect : IReturn<DoctorsSelectResponse>
    {
    }

    public class DoctorsSelectResponse : IHasResponseStatus
    {
        public DoctorsSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<Doctor> Doctors { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/doctor/save", "POST")]
    public class DoctorSave : IReturn<DoctorSaveResponse>
    {
        public Doctor Doctor { get; set; }
    }

    public class DoctorSaveResponse : IHasResponseStatus
    {
        public DoctorSaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public int DoctorId { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/Doctor/delete/{DoctorId}", "GET")]
    public class DoctorDelete : IReturn<DoctorDeleteResponse>
    {
        public int DoctorId { get; set; }
    }

    public class DoctorDeleteResponse : IHasResponseStatus
    {
        public DoctorDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    #endregion


    #region Inspections In Doctors

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/doctors/bind/{InspectionId}/{DoctorId}", "GET")]
    public class InspectionsDoctorsBind : IReturn<InspectionsDoctorsBindResponse>
    {
        public int InspectionId { get; set; }

        public int DoctorId { get; set; }

    }

    public class InspectionsDoctorsBindResponse : IHasResponseStatus
    {
        public InspectionsDoctorsBindResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/Doctors/unbind/{InspectionId}/{DoctorId}", "GET")]
    public class InspectionsDoctorsUnbind : IReturn<InspectionsDoctorsUnbindResponse>
    {
        public int InspectionId { get; set; }

        public int DoctorId { get; set; }

    }

    public class InspectionsDoctorsUnbindResponse : IHasResponseStatus
    {
        public InspectionsDoctorsUnbindResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [Authenticate]
    [Route("/inspectionsindoctor/{DoctorId}", "GET")]
    public class InspectionsInDoctorSelect : IReturn<InspectionsInDoctorSelectResponse>
    {
        public int DoctorId { get; set; }
    }

    public class InspectionsInDoctorSelectResponse : IHasResponseStatus
    {
        public InspectionsInDoctorSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<int> InspectionIds { get; set; }
    }

    [Authenticate]
    [Route("/doctorsininspection/{InspectionId}", "GET")]
    public class DoctorsInInspectionSelect : IReturn<DoctorsInInspectionSelectResponse>
    {
        public int DoctorId { get; set; }
    }

    public class DoctorsInInspectionSelectResponse : IHasResponseStatus
    {
        public DoctorsInInspectionSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<int> DoctorIds { get; set; }
    }

    #endregion
}
