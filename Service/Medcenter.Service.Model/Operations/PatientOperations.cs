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
    #region Patient

    [Authenticate]
    [Route("/patients/{Text}", "GET")]
    public class PatientsSelect : IReturn<PatientsSelectResponse>
    {
        public string Text { get; set; }
    }

    public class PatientsSelectResponse : IHasResponseStatus
    {
        public PatientsSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<Patient> Patients { get; set; }
    }
    [Authenticate]
    [Route("/patient/{Id}", "GET")]
    public class PatientSelect : IReturn<PatientSelectResponse>
    {
        public int Id { get; set; }
    }

    public class PatientSelectResponse : IHasResponseStatus
    {
        public PatientSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public Patient Patient { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/patient/save", "POST")]
    public class PatientSave : IReturn<PatientSaveResponse>
    {
        public Patient Patient { get; set; }
    }

    public class PatientSaveResponse : IHasResponseStatus
    {
        public PatientSaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public int PatientId { get; set; }
    }
    [RequiresAnyRole("Admin", "Manager","Nurse")]
    [Route("/patient/clarify", "POST")]
    public class PatientClarify : IReturn<PatientClarifyResponse>
    {
        public Patient Patient { get; set; }
    }

    public class PatientClarifyResponse : IHasResponseStatus
    {
        public PatientClarifyResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
    [RequiresAnyRole("Admin", "Manager")]
    [Route("/patient/delete/{PatientId}", "GET")]
    public class PatientDelete : IReturn<PatientDeleteResponse>
    {
        public int PatientId { get; set; }
    }

    public class PatientDeleteResponse : IHasResponseStatus
    {
        public PatientDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    #endregion

    #region City

    [Route("/cities", "GET")]
    public class CitiesSelect : IReturn<CitiesSelectResponse>
    {
    }

    public class CitiesSelectResponse : IHasResponseStatus
    {
        public CitiesSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<City> Cities { get; set; }
    }
    
    [RequiresAnyRole("Admin", "Manager")]
    [Route("/city/save", "POST")]
    public class CitySave : IReturn<CitySaveResponse>
    {
        public City City { get; set; }
    }

    public class CitySaveResponse : IHasResponseStatus
    {
        public CitySaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public int CityId { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/city/delete/{CityId}", "GET")]
    public class CityDelete : IReturn<CityDeleteResponse>
    {
        public int CityId { get; set; }
    }

    public class CityDeleteResponse : IHasResponseStatus
    {
        public CityDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    #endregion


}
