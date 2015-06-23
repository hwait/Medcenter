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


    #region Packages In Doctors

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/doctors/bind/{PackageId}/{DoctorId}", "GET")]
    public class PackagesDoctorsBind : IReturn<PackagesDoctorsBindResponse>
    {
        public int PackageId { get; set; }

        public int DoctorId { get; set; }

    }

    public class PackagesDoctorsBindResponse : IHasResponseStatus
    {
        public PackagesDoctorsBindResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/Doctors/unbind/{PackageId}/{DoctorId}", "GET")]
    public class PackagesDoctorsUnbind : IReturn<PackagesDoctorsUnbindResponse>
    {
        public int PackageId { get; set; }

        public int DoctorId { get; set; }

    }

    public class PackagesDoctorsUnbindResponse : IHasResponseStatus
    {
        public PackagesDoctorsUnbindResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [Authenticate]
    [Route("/Packagesindoctor/{DoctorId}", "GET")]
    public class PackagesInDoctorSelect : IReturn<PackagesInDoctorSelectResponse>
    {
        public int DoctorId { get; set; }
    }

    public class PackagesInDoctorSelectResponse : IHasResponseStatus
    {
        public PackagesInDoctorSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<int> PackageIds { get; set; }
    }

    [Authenticate]
    [Route("/doctorsinPackage/{PackageId}", "GET")]
    public class DoctorsInPackageSelect : IReturn<DoctorsInPackageSelectResponse>
    {
        public int DoctorId { get; set; }
    }

    public class DoctorsInPackageSelectResponse : IHasResponseStatus
    {
        public DoctorsInPackageSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<int> DoctorIds { get; set; }
    }

    #endregion
}
