using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using ServiceStack.Auth;
using ServiceStack.OrmLite;
using ServiceStack;

namespace Medcenter.Service.Interface.Services
{
    public class InspectionService : ServiceStack.Service
    {
        /*
         InspectionsSelectResponse
         * InspectionSaveResponse
         * InspectionDeleteResponse
         * 
         * InspectionGroupsSelectResponse
         * InspectionGroupSaveResponse
         * InspectionGroupDeleteResponse
         * 
         * InspectionsGroupsBindResponse
         * InspectionsGroupsUnbindResponse
         * 
         * InspectionsInGroupSelectResponse
         * GroupsInInspectionSelectResponse
         */
        #region Inspections and Groups

        public InspectionsInGroupSelectResponse Get(InspectionsInGroupSelect req)
        {
            var rows = Db.SqlList<int>("EXEC sp_InspectionGroup_SelectInspections @Id", new
            {
                Id = req.InspectionGroupId
            });

            return new InspectionsInGroupSelectResponse { InspectionIds = new List<int>(rows) };
        }
        public GroupsInInspectionSelectResponse Get(GroupsInInspectionSelect req)
        {
            var rows = Db.SqlList<int>("EXEC sp_Inspection_SelectGroups @Id", new
            {
                Id = req.InspectionGroupId
            });

            return new GroupsInInspectionSelectResponse { InspectionGroupIds = new List<int>(rows) };
        }
        public InspectionsGroupsBindResponse Post(InspectionsGroupsBind req)
        {
            ResultMessage _message;
                try
                {
                    var rows = Db.SqlList<int>("EXEC sp_InspectionsInGroups_Insert @InspectionId, @InspectionGroupId", new
                    {
                        InspectionId = req.InspectionId,
                        InspectionGroupId = req.InspectionGroupId
                    });
                    _message = new ResultMessage(0, "Связывание", OperationResults.InspectionsGroupsBind);
                    Logger.Log("InspectionsGroups.Bind");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.InspectionsGroupsBind);
                    Logger.Log("InspectionsGroups.Bind", e);
                    throw;
                }
            return new InspectionsGroupsBindResponse
            {
                Message = _message
            };
        }

        public InspectionsGroupsUnbindResponse Post(InspectionsGroupsUnbind req)
        {
            ResultMessage _message;
            try
            {
                var rows = Db.SqlList<int>("EXEC sp_InspectionsInGroups_Delete @InspectionId, @InspectionGroupId", new
                {
                    InspectionId = req.InspectionId,
                    InspectionGroupId = req.InspectionGroupId
                });
                _message = new ResultMessage(0, "Связывание", OperationResults.InspectionsGroupsUnbind);
                Logger.Log("InspectionsGroups.UnBind");
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.InspectionsGroupsUnbind);
                Logger.Log("InspectionsGroups.UnBind", e);
                throw;
            }
            return new InspectionsGroupsUnbindResponse
            {
                Message = _message
            };
        }

        #endregion
        #region InspectionGroup

        public InspectionGroupsSelectResponse Get(InspectionGroupsSelect req)
        {
            var rows = Db.SqlList<InspectionGroup>("EXEC sp_InspectionGroups_Select");
            foreach (var r in rows)
                r.InspectionIds = Db.SqlList<int>("EXEC sp_InspectionGroup_SelectInspections @InspectionGroupId", new { InspectionGroupId = r.Id });
            return new InspectionGroupsSelectResponse { InspectionGroups = new List<InspectionGroup>(rows) };
        }

        public InspectionGroupSaveResponse Post(InspectionGroupSave req)
        {
            int id = 0;
            ResultMessage _message;
            if (req.InspectionGroup.Id > 0) // Inspection exists so we're saving 
            {
                try
                {
                    id = Db.Single<int>("EXEC sp_InspectionGroups_Update @Id, @ShortName, @Name, @Row, @Color", new
                    {
                        Id = req.InspectionGroup.Id,
                        ShortName = req.InspectionGroup.ShortName,
                        Name = req.InspectionGroup.Name,
                        Color = req.InspectionGroup.Color,
                        Row = req.InspectionGroup.Row,
                    });
                    _message = new ResultMessage(0, "Сохранение исследования", OperationResults.InspectionGroupSave);
                    Logger.Log("InspectionSaveResponse.Saving");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.InspectionGroupSave);
                    Logger.Log("InspectionSaveResponse.Saving", e);
                    throw;
                }
            }
            else //New InspectionGroup
            {
                try
                {
                    id = Db.Single<int>("EXEC sp_InspectionGroups_Insert @ShortName, @Name, @Row, @Color", new
                    {
                        ShortName = req.InspectionGroup.ShortName,
                        Name = req.InspectionGroup.Name,
                        Color = req.InspectionGroup.Color,
                        Row = req.InspectionGroup.Row,
                    });
                    _message = new ResultMessage(0, "Новое исследование", OperationResults.InspectionGroupCreate);
                    Logger.Log("InspectionSaveResponse.NewInspection");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.InspectionGroupCreate);
                    Logger.Log("InspectionSaveResponse.NewInspection", e);
                    throw;
                }
            }

            return new InspectionGroupSaveResponse
            {
                InspectionGroupId = id,
                Message = _message
            };
        }

        public InspectionGroupDeleteResponse Get(InspectionGroupDelete req)
        {
            ResultMessage _message;
            try
            {
                var res = Db.SqlList<int>("EXEC sp_InspectionGroups_Delete @Id", new
                {
                    Id = req.InspectionGroupId
                });
                _message = new ResultMessage(0, "Инспекции:", OperationResults.InspectionGroupDelete);
                Logger.Log("InspectionGroupDeleteResponse");
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.InspectionGroupDelete);
                Logger.Log("InspectionGroupDeleteResponse", e);
                throw;
            }
            return new InspectionGroupDeleteResponse { Message = _message };
        }

        #endregion
        #region Inspection

        public InspectionsSelectResponse Get(InspectionsSelect req)
        {
            var rows = Db.SqlList<Inspection>("EXEC sp_Inspections_Select");
            foreach (var r in rows)
                r.InspectionGroupIds = Db.SqlList<int>("EXEC sp_Inspection_SelectGroups @InspectionId", new { InspectionId = r.Id });
            return new InspectionsSelectResponse {Inspections = new List<Inspection>(rows)};
        }

        public InspectionSaveResponse Post(InspectionSave req)
        {
            int id = 0;
            ResultMessage _message;
            if (req.Inspection.Id > 0) // Inspection exists so we're saving 
            {
                try
                {
                    id = Db.Single<int>("EXEC sp_Inspections_Update @Id, @ShortName, @Name, @Duration, @Cost", new
                    {
                        Id = req.Inspection.Id,
                        ShortName = req.Inspection.ShortName,
                        Name = req.Inspection.Name,
                        Duration = req.Inspection.Duration,
                        Cost = req.Inspection.Cost,
                    });
                    _message = new ResultMessage(0, "Сохранение исследования", OperationResults.InspectionSave);
                    Logger.Log("InspectionSaveResponse.Saving");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.InspectionSave);
                    Logger.Log("InspectionSaveResponse.Saving", e);
                    throw;
                }
            }
            else //New Inspection
            {
                try
                {
                    id = Db.Single<int>("EXEC sp_Inspections_Insert @ShortName, @Name, @Duration, @Cost", new
                    {
                        ShortName = req.Inspection.ShortName,
                        Name = req.Inspection.Name,
                        Duration = req.Inspection.Duration,
                        Cost = req.Inspection.Cost,
                    });
                    _message = new ResultMessage(0, "Новое исследование", OperationResults.InspectionCreate);
                    Logger.Log("InspectionSaveResponse.NewInspection");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.InspectionCreate);
                    Logger.Log("InspectionSaveResponse.NewInspection", e);
                    throw;
                }
            }

            return new InspectionSaveResponse
            {
                InspectionId = id,
                Message = _message
            };
        }

        public InspectionDeleteResponse Get(InspectionDelete req)
        {
            ResultMessage _message;
            try
            {
                var res = Db.SqlList<int>("EXEC sp_Inspections_Delete @Id", new
                {
                    Id = req.InspectionId
                });
                _message = new ResultMessage(0, "Инспекции:", OperationResults.InspectionDelete);
                Logger.Log("InspectionDeleteResponse");
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.InspectionDelete);
                Logger.Log("InspectionDeleteResponse", e);
                throw;
            }
            return new InspectionDeleteResponse {Message = _message};
        }

        #endregion

    }
}
