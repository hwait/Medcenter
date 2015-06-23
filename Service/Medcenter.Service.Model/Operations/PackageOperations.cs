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

    #region Package

    [Authenticate]
    [Route("/Packages", "GET")]
    public class PackagesSelect : IReturn<PackagesSelectResponse>
    {
    }

    public class PackagesSelectResponse : IHasResponseStatus
    {
        public PackagesSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<Package> Packages { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/Package/save", "POST")]
    public class PackageSave : IReturn<PackageSaveResponse>
    {
        public Package Package { get; set; }
    }

    public class PackageSaveResponse : IHasResponseStatus
    {
        public PackageSaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public int PackageId { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/Package/delete/{PackageId}", "GET")]
    public class PackageDelete : IReturn<PackageDeleteResponse>
    {
        public int PackageId { get; set; }
    }

    public class PackageDeleteResponse : IHasResponseStatus
    {
        public PackageDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }


    #endregion


    #region PackageGroup

    [Authenticate]
    [Route("/Packagegroups", "GET")]
    public class PackageGroupsSelect : IReturn<PackageGroupsSelectResponse>
    {
    }

    public class PackageGroupsSelectResponse : IHasResponseStatus
    {
        public PackageGroupsSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<PackageGroup> PackageGroups { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/Packagegroup/save", "POST")]
    public class PackageGroupSave : IReturn<PackageGroupSaveResponse>
    {
        public PackageGroup PackageGroup { get; set; }
    }

    public class PackageGroupSaveResponse : IHasResponseStatus
    {
        public PackageGroupSaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public int PackageGroupId { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/Packagegroup/delete/{PackageGroupId}", "GET")]
    public class PackageGroupDelete : IReturn<PackageGroupDeleteResponse>
    {
        public int PackageGroupId { get; set; }
    }

    public class PackageGroupDeleteResponse : IHasResponseStatus
    {
        public PackageGroupDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    #endregion


    #region Packages In Groups

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/Packagegroups/bind/{PackageId}/{PackageGroupId}", "GET")]
    public class PackagesGroupsBind : IReturn<PackagesGroupsBindResponse>
    {
        public int PackageId { get; set; }

        public int PackageGroupId { get; set; }

    }

    public class PackagesGroupsBindResponse : IHasResponseStatus
    {
        public PackagesGroupsBindResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/Packagegroups/unbind/{PackageId}/{PackageGroupId}", "GET")]
    public class PackagesGroupsUnbind : IReturn<PackagesGroupsUnbindResponse>
    {
        public int PackageId { get; set; }

        public int PackageGroupId { get; set; }

    }

    public class PackagesGroupsUnbindResponse : IHasResponseStatus
    {
        public PackagesGroupsUnbindResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [Authenticate]
    [Route("/Packagesingroup/{PackageGroupId}", "GET")]
    public class PackagesInGroupSelect : IReturn<PackagesInGroupSelectResponse>
    {
        public int PackageGroupId { get; set; }
    }

    public class PackagesInGroupSelectResponse : IHasResponseStatus
    {
        public PackagesInGroupSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<int> PackageIds { get; set; }
    }

    [Authenticate]
    [Route("/groupsinPackage/{PackageId}", "GET")]
    public class GroupsInPackageSelect : IReturn<GroupsInPackageSelectResponse>
    {
        public int PackageGroupId { get; set; }
    }

    public class GroupsInPackageSelectResponse : IHasResponseStatus
    {
        public GroupsInPackageSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<int> PackageGroupIds { get; set; }
    }

    #endregion
}

