using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using ServiceStack.Auth;
using ServiceStack.OrmLite;
using ServiceStack;


namespace Medcenter.Service.Interface.Services
{
    public class PackageService : ServiceStack.Service
    {
        /*
         PackagesSelectResponse
         * PackageSaveResponse
         * PackageDeleteResponse
         * 
         * PackageGroupsSelectResponse
         * PackageGroupSaveResponse
         * PackageGroupDeleteResponse
         * 
         * PackagesGroupsBindResponse
         * PackagesGroupsUnbindResponse
         * 
         * PackagesInGroupSelectResponse
         * GroupsInPackageSelectResponse
         */
        #region Packages and Groups

        public PackagesInGroupSelectResponse Get(PackagesInGroupSelect req)
        {
            var rows = Db.SqlList<int>("EXEC sp_PackageGroup_SelectPackages @Id", new
            {
                Id = req.PackageGroupId
            });

            return new PackagesInGroupSelectResponse { PackageIds = new List<int>(rows) };
        }
        public GroupsInPackageSelectResponse Get(GroupsInPackageSelect req)
        {
            var rows = Db.SqlList<int>("EXEC sp_Package_SelectGroups @Id", new
            {
                Id = req.PackageGroupId
            });

            return new GroupsInPackageSelectResponse { PackageGroupIds = new List<int>(rows) };
        }
        public PackagesGroupsBindResponse Get(PackagesGroupsBind req)
        {
            ResultMessage _message;
                try
                {
                    var rows = Db.SqlList<int>("EXEC sp_PackagesInGroups_Insert @PackageId, @PackageGroupId", new
                    {
                        PackageId = req.PackageId,
                        PackageGroupId = req.PackageGroupId
                    });
                    if (rows[0] == 0)
                    {
                        _message = new ResultMessage(2, "Связывание", OperationErrors.PackagesGroupsBindZero);
                        Logger.Log("PackagesGroups.Bind 0");
                    }
                    else
                    {
                        _message = new ResultMessage(0, "Связывание", OperationResults.PackagesGroupsBind);
                        Logger.Log("PackagesGroups.Bind 1");
                    }
                   
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.PackagesGroupsBind);
                    Logger.Log("PackagesGroups.Bind", e);
                    throw;
                }
            return new PackagesGroupsBindResponse
            {
                Message = _message
            };
        }

        public PackagesGroupsUnbindResponse Get(PackagesGroupsUnbind req)
        {
            ResultMessage _message;
            try
            {
                var rows = Db.SqlList<int>("EXEC sp_PackagesInGroups_Delete @PackageId, @PackageGroupId", new
                {
                    PackageId = req.PackageId,
                    PackageGroupId = req.PackageGroupId
                });
                if (rows[0] == 0)
                {
                    _message = new ResultMessage(2, "Отвязывание", OperationErrors.PackagesGroupsUnbindZero);
                    Logger.Log("PackagesGroups.UnBind");
                }
                else
                {
                    _message = new ResultMessage(0, "Отвязывание", OperationResults.PackagesGroupsUnbind);
                    Logger.Log("PackagesGroups.UnBind");
                }
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.PackagesGroupsUnbind);
                Logger.Log("PackagesGroups.UnBind", e);
                throw;
            }
            return new PackagesGroupsUnbindResponse
            {
                Message = _message
            };
        }

        #endregion
        #region PackageGroup

        public PackageGroupsSelectResponse Get(PackageGroupsSelect req)
        {
            var rows = Db.SqlList<PackageGroup>("EXEC sp_PackageGroups_Select");
            foreach (var r in rows)
                r.PackageIds = Db.SqlList<int>("EXEC sp_PackageGroup_SelectPackages @PackageGroupId", new { PackageGroupId = r.Id });
            return new PackageGroupsSelectResponse { PackageGroups = new List<PackageGroup>(rows) };
        }

        public PackageGroupSaveResponse Post(PackageGroupSave req)
        {
            int id = 0;
            ResultMessage _message;
            if (req.PackageGroup.Id > 0) // Package exists so we're saving 
            {
                try
                {
                    id = Db.Single<int>("EXEC sp_PackageGroups_Update @Id, @ShortName, @Name, @Row, @Color", new
                    {
                        Id = req.PackageGroup.Id,
                        ShortName = req.PackageGroup.ShortName,
                        Name = req.PackageGroup.Name,
                        Color = req.PackageGroup.Color,
                        Row = req.PackageGroup.Row,
                    });
                    _message = new ResultMessage(0, "Сохранение исследования", OperationResults.PackageGroupSave);
                    Logger.Log("PackageGroupSaveResponse.Saving");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.PackageGroupSave);
                    Logger.Log("PackageGroupSaveResponse.Saving " , e);
                    throw;
                }
            }
            else //New PackageGroup
            {
                try
                {
                    id = Db.Single<int>("EXEC sp_PackageGroups_Insert @ShortName, @Name, @Row, @Color", new
                    {
                        ShortName = req.PackageGroup.ShortName,
                        Name = req.PackageGroup.Name,
                        Color = req.PackageGroup.Color,
                        Row = req.PackageGroup.Row,
                    });
                    _message = new ResultMessage(0, "Новое исследование", OperationResults.PackageGroupCreate);
                    Logger.Log("PackageSaveResponse.NewPackage");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.PackageGroupCreate);
                    Logger.Log("PackageSaveResponse.NewPackage", e);
                    throw;
                }
            }

            return new PackageGroupSaveResponse
            {
                PackageGroupId = id,
                Message = _message
            };
        }

        public PackageGroupDeleteResponse Get(PackageGroupDelete req)
        {
            ResultMessage _message;
            try
            {
                var res = Db.SqlList<int>("EXEC sp_PackageGroups_Delete @Id", new
                {
                    Id = req.PackageGroupId
                });
                _message = new ResultMessage(0, "Инспекции:", OperationResults.PackageGroupDelete);
                Logger.Log("PackageGroupDeleteResponse");
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.PackageGroupDelete);
                Logger.Log("PackageGroupDeleteResponse", e);
                throw;
            }
            return new PackageGroupDeleteResponse { Message = _message };
        }

        #endregion
        #region Package

        public PackagesSelectResponse Get(PackagesSelect req)
        {
            var rows = Db.SqlList<Package>("EXEC sp_Packages_Select");
            foreach (var r in rows)
                r.PackageGroupIds = Db.SqlList<int>("EXEC sp_Package_SelectGroups @PackageId", new { PackageId = r.Id });
            return new PackagesSelectResponse {Packages = new List<Package>(rows)};
        }

        public PackageSaveResponse Post(PackageSave req)
        {
            int id = 0;
            ResultMessage _message;
            if (req.Package.Id > 0) // Package exists so we're saving 
            {
                try
                {
                    id = Db.Single<int>("EXEC sp_Packages_Update @Id, @ShortName, @Name, @Duration, @Cost", new
                    {
                        Id = req.Package.Id,
                        ShortName = req.Package.ShortName,
                        Name = req.Package.Name,
                        Duration = req.Package.Duration,
                        Cost = req.Package.Cost,
                    });
                    _message = new ResultMessage(0, "Сохранение исследования", OperationResults.PackageSave);
                    Logger.Log("PackageSaveResponse.Saving");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.PackageSave);
                    Logger.Log("PackageSaveResponse.Saving", e);
                    throw;
                }
            }
            else //New Package
            {
                try
                {
                    id = Db.Single<int>("EXEC sp_Packages_Insert @ShortName, @Name, @Duration, @Cost", new
                    {
                        ShortName = req.Package.ShortName,
                        Name = req.Package.Name,
                        Duration = req.Package.Duration,
                        Cost = req.Package.Cost,
                    });
                    _message = new ResultMessage(0, "Новое исследование", OperationResults.PackageCreate);
                    Logger.Log("PackageSaveResponse.NewPackage");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.PackageCreate);
                    Logger.Log("PackageSaveResponse.NewPackage", e);
                    throw;
                }
            }

            return new PackageSaveResponse
            {
                PackageId = id,
                Message = _message
            };
        }

        public PackageDeleteResponse Get(PackageDelete req)
        {
            ResultMessage _message;
            try
            {
                var res = Db.SqlList<int>("EXEC sp_Packages_Delete @Id", new
                {
                    Id = req.PackageId
                });
                _message = new ResultMessage(0, "Инспекции:", OperationResults.PackageDelete);
                Logger.Log("PackageDeleteResponse");
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.PackageDelete);
                Logger.Log("PackageDeleteResponse", e);
                throw;
            }
            return new PackageDeleteResponse {Message = _message};
        }

        #endregion

    }
}
