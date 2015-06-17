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


    #region InspectionGroup

    [Authenticate]
    [Route("/inspectiongroups", "GET")]
    public class InspectionGroupsSelect : IReturn<InspectionGroupsSelectResponse>
    {
    }

    public class InspectionGroupsSelectResponse : IHasResponseStatus
    {
        public InspectionGroupsSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<InspectionGroup> InspectionGroups { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/inspectiongroup/save", "POST")]
    public class InspectionGroupSave : IReturn<InspectionGroupSaveResponse>
    {
        public InspectionGroup InspectionGroup { get; set; }
    }

    public class InspectionGroupSaveResponse : IHasResponseStatus
    {
        public InspectionGroupSaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public int InspectionGroupId { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/inspectiongroup/delete/{InspectionGroupId}", "GET")]
    public class InspectionGroupDelete : IReturn<InspectionGroupDeleteResponse>
    {
        public int InspectionGroupId { get; set; }
    }

    public class InspectionGroupDeleteResponse : IHasResponseStatus
    {
        public InspectionGroupDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    #endregion


    #region Inspections In Groups

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/inspectiongroups/bind/{InspectionId}/{InspectionGroupId}", "GET")]
    public class InspectionsGroupsBind : IReturn<InspectionsGroupsBindResponse>
    {
        public int InspectionId { get; set; }

        public int InspectionGroupId { get; set; }

    }

    public class InspectionsGroupsBindResponse : IHasResponseStatus
    {
        public InspectionsGroupsBindResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [RequiresAnyRole("Admin", "Manager")]
    [Route("/inspectiongroups/unbind/{InspectionId}/{InspectionGroupId}", "GET")]
    public class InspectionsGroupsUnbind : IReturn<InspectionsGroupsUnbindResponse>
    {
        public int InspectionId { get; set; }

        public int InspectionGroupId { get; set; }

    }

    public class InspectionsGroupsUnbindResponse : IHasResponseStatus
    {
        public InspectionsGroupsUnbindResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [Authenticate]
    [Route("/inspectionsingroup/{InspectionGroupId}", "GET")]
    public class InspectionsInGroupSelect : IReturn<InspectionsInGroupSelectResponse>
    {
        public int InspectionGroupId { get; set; }
    }

    public class InspectionsInGroupSelectResponse : IHasResponseStatus
    {
        public InspectionsInGroupSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<int> InspectionIds { get; set; }
    }

    [Authenticate]
    [Route("/groupsininspection/{InspectionId}", "GET")]
    public class GroupsInInspectionSelect : IReturn<GroupsInInspectionSelectResponse>
    {
        public int InspectionGroupId { get; set; }
    }

    public class GroupsInInspectionSelectResponse : IHasResponseStatus
    {
        public GroupsInInspectionSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }

        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<int> InspectionGroupIds { get; set; }
    }

    #endregion
}

