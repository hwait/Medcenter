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


    #region Inspection

    [Authenticate]
    [Route("/inspections", "GET")]
    public class InspectionsSelect : IReturn<InspectionsSelectResponse>
    {
    }

    public class InspectionsSelectResponse : IHasResponseStatus
    {
        public InspectionsSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<Inspection> Inspections { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/inspection/save", "POST")]
    public class InspectionSave : IReturn<InspectionSaveResponse>
    {
        public Inspection Inspection { get; set; }
    }

    public class InspectionSaveResponse : IHasResponseStatus
    {
        public InspectionSaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public int InspectionId { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/inspection/delete/{InspectionId}", "GET")]
    public class InspectionDelete : IReturn<InspectionDeleteResponse>
    {
        public int InspectionId { get; set; }
    }

    public class InspectionDeleteResponse : IHasResponseStatus
    {
        public InspectionDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    #endregion


    #region  Inspectoins In Packages

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/inspectionpackage/bind/{InspectionId}/{PackageId}", "GET")]
    public class InspectionsPackagesBind : IReturn<InspectionsPackagesBindResponse>
    {
        public int PackageId { get; set; }

        public int InspectionId { get; set; }

    }

    public class InspectionsPackagesBindResponse : IHasResponseStatus
    {
        public InspectionsPackagesBindResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/inspectionpackage/unbind/{InspectionId}/{PackageId}", "GET")]
    public class InspectionsPackagesUnbind : IReturn<InspectionsPackagesUnbindResponse>
    {
        public int PackageId { get; set; }

        public int InspectionId { get; set; }

    }

    public class InspectionsPackagesUnbindResponse : IHasResponseStatus
    {
        public InspectionsPackagesUnbindResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [Authenticate]
    [Route("/inspectionsinpackage/{PackageId}", "GET")]
    public class InspectionsInPackageSelect : IReturn<InspectionsInPackageSelectResponse>
    {
        public int PackageId { get; set; }
    }

    public class InspectionsInPackageSelectResponse : IHasResponseStatus
    {
        public InspectionsInPackageSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<int> InspectionIds { get; set; }
    }

    [Authenticate]
    [Route("/packagesininspection/{InspectionId}", "GET")]
    public class PackagesInInspectionSelect : IReturn<PackagesInInspectionSelectResponse>
    {
        public int InspectionId { get; set; }
    }

    public class PackagesInInspectionSelectResponse : IHasResponseStatus
    {
        public PackagesInInspectionSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<int> PackageIds { get; set; }
    }

    #endregion


    //#region Discount

    //[Authenticate]
    //[Route("/discounts", "GET")]
    //public class DiscountsSelect : IReturn<DiscountsSelectResponse>
    //{
    //}

    //public class DiscountsSelectResponse : IHasResponseStatus
    //{
    //    public DiscountsSelectResponse()
    //    {
    //        ResponseStatus = new ResponseStatus();
    //    }

    //    public ResultMessage Message { get; set; }
    //    public ResponseStatus ResponseStatus { get; set; }
    //    public List<Discount> Discounts { get; set; }
    //}

    //[RequiresAnyRole("Admin", "Manager")]
    //[Route("/discount/save", "POST")]
    //public class DiscountSave : IReturn<DiscountSaveResponse>
    //{
    //    public Discount Discount { get; set; }
    //}

    //public class DiscountSaveResponse : IHasResponseStatus
    //{
    //    public DiscountSaveResponse()
    //    {
    //        ResponseStatus = new ResponseStatus();
    //    }

    //    public ResultMessage Message { get; set; }
    //    public ResponseStatus ResponseStatus { get; set; }
    //    public int DiscountId { get; set; }
    //}

    //[RequiresAnyRole("Admin", "Manager")]
    //[Route("/discount/delete/{DiscountId}", "GET")]
    //public class DiscountDelete : IReturn<DiscountDeleteResponse>
    //{
    //    public int DiscountId { get; set; }
    //}

    //public class DiscountDeleteResponse : IHasResponseStatus
    //{
    //    public DiscountDeleteResponse()
    //    {
    //        ResponseStatus = new ResponseStatus();
    //    }

    //    public ResultMessage Message { get; set; }
    //    public ResponseStatus ResponseStatus { get; set; }
    //}

    //#endregion


    #region Discounts In Package

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/discountpackage/bind/{DiscountId}/{PackageId}", "GET")]
    public class DiscountsPackagesBind : IReturn<DiscountsPackagesBindResponse>
    {
        public int PackageId { get; set; }

        public int DiscountId { get; set; }

    }

    public class DiscountsPackagesBindResponse : IHasResponseStatus
    {
        public DiscountsPackagesBindResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/discountpackage/unbind/{DiscountId}/{PackageId}", "GET")]
    public class DiscountsPackagesUnbind : IReturn<DiscountsPackagesUnbindResponse>
    {
        public int PackageId { get; set; }

        public int DiscountId { get; set; }

    }

    public class DiscountsPackagesUnbindResponse : IHasResponseStatus
    {
        public DiscountsPackagesUnbindResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [Authenticate]
    [Route("/discountinpackage/{PackageId}", "GET")]
    public class DiscountsInPackageSelect : IReturn<DiscountsInPackageSelectResponse>
    {
        public int PackageId { get; set; }
    }

    public class DiscountsInPackageSelectResponse : IHasResponseStatus
    {
        public DiscountsInPackageSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<int> DiscountIds { get; set; }
    }

    [Authenticate]
    [Route("/packageindiscount/{DiscountId}", "GET")]
    public class PackagesInDiscountSelect : IReturn<PackagesInDiscountSelectResponse>
    {
        public int DiscountId { get; set; }
    }

    public class PackagesInDiscountSelectResponse : IHasResponseStatus
    {
        public PackagesInDiscountSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<int> PackageIds { get; set; }
    }

    #endregion
}

