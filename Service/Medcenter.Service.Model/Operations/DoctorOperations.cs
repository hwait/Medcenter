using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Types;
using ServiceStack;

namespace Medcenter.Service.Model.Operations
{
    [Route("/doctors")]
    public class DoctorSelect : IReturn<DoctorSelectResponse>
    {

    }
    public class DoctorSelectResponse : IHasResponseStatus
    {
        public DoctorSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResponseStatus ResponseStatus { get; set; }
        public List<Doctor> Doctors { get; set; }

    }
    [Route("/doctor/new")]
    public class DoctorInsert : IReturn<DoctorInsertResponse>
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
    public class DoctorInsertResponse : IHasResponseStatus
    {
        public DoctorInsertResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResponseStatus ResponseStatus { get; set; }
        public int Id { get; set; }

    }
    [Route("/doctor/update")]
    public class DoctorUpdate : IReturn<DoctorUpdateResponse>
    {
        public Doctor Doctor { get; set; }
    }
    public class DoctorUpdateResponse : IHasResponseStatus
    {
        public DoctorUpdateResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResponseStatus ResponseStatus { get; set; }
    }
    [Route("/doctor/delete/{Id}")]
    public class DoctorDelete : IReturn<DoctorInsertResponse>
    {
        public int Id { get; set; }
    }
    public class DoctorDeleteResponse : IHasResponseStatus
    {
        public DoctorDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
