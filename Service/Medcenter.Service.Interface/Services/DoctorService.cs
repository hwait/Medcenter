using System;
using System.Collections.Generic;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using ServiceStack.OrmLite;

namespace Medcenter.Service.Interface.Services
{
    public class DoctorService : ServiceStack.Service
    {
        public NursesSelectResponse Get(NursesSelect req)
        {
            var rows = Db.SqlList<Nurse>("EXEC sp_Nurses_Select");
            foreach (var r in rows)
                r.DoctorIds = Db.SqlList<int>("EXEC sp_Nurse_SelectDoctors @NurseId", new { NurseId = r.Id });
            return new NursesSelectResponse { Nurses = new List<Nurse>(rows) };
        }
        #region Doctor

        public DoctorsSelectResponse Get(DoctorsSelect req)
        {
            var rows = Db.SqlList<Doctor>("EXEC sp_Doctors_Select");
            foreach (var r in rows)
                r.PackageIds = Db.SqlList<int>("EXEC sp_Doctor_SelectPackages @DoctorId", new { DoctorId = r.Id });
            return new DoctorsSelectResponse { Doctors = new List<Doctor>(rows) };
        }

        public DoctorSaveResponse Post(DoctorSave req)
        {
            int id = 0;
            ResultMessage _message;
            if (req.Doctor.Id > 0) // Package exists so we're saving 
            {
                try
                {
                    id = Db.Single<int>("EXEC sp_Doctors_Update @Id, @ShortName, @Name, @Color", new
                    {
                        Id = req.Doctor.Id,
                        ShortName = req.Doctor.ShortName,
                        Name = req.Doctor.Name,
                        Color = req.Doctor.Color
                    });
                    _message = new ResultMessage(0, "Сохранение исследования", OperationResults.DoctorSave);
                    Logger.Log("DoctorSaveResponse.Saving");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.DoctorSave);
                    Logger.Log("DoctorSaveResponse.Saving ", e);
                    throw;
                }
            }
            else //New Doctor
            {
                try
                {
                    id = Db.Single<int>("EXEC sp_Doctors_Insert @ShortName, @Name, @Color", new
                    {
                        ShortName = req.Doctor.ShortName,
                        Name = req.Doctor.Name,
                        Color = req.Doctor.Color
                    });
                    _message = new ResultMessage(0, "Новое исследование", OperationResults.DoctorCreate);
                    Logger.Log("PackageSaveResponse.NewPackage");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.DoctorCreate);
                    Logger.Log("PackageSaveResponse.NewPackage", e);
                    throw;
                }
            }

            return new DoctorSaveResponse
            {
                DoctorId = id,
                Message = _message
            };
        }

        public DoctorDeleteResponse Get(DoctorDelete req)
        {
            ResultMessage _message;
            try
            {
                var res = Db.SqlList<int>("EXEC sp_Doctors_Delete @Id", new
                {
                    Id = req.DoctorId
                });
                _message = new ResultMessage(0, "Инспекции:", OperationResults.DoctorDelete);
                Logger.Log("DoctorDeleteResponse");
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.DoctorDelete);
                Logger.Log("DoctorDeleteResponse", e);
                throw;
            }
            return new DoctorDeleteResponse { Message = _message };
        }

        #endregion 

        #region Packages and Doctors

        public PackagesInDoctorSelectResponse Get(PackagesInDoctorSelect req)
        {
            var rows = Db.SqlList<int>("EXEC sp_Doctor_SelectPackages @Id", new
            {
                Id = req.DoctorId
            });

            return new PackagesInDoctorSelectResponse { PackageIds = new List<int>(rows) };
        }
        public DoctorsInPackageSelectResponse Get(DoctorsInPackageSelect req)
        {
            var rows = Db.SqlList<int>("EXEC sp_Package_SelectDoctors @Id", new
            {
                Id = req.DoctorId
            });

            return new DoctorsInPackageSelectResponse { DoctorIds = new List<int>(rows) };
        }
        public PackagesDoctorsBindResponse Get(PackagesDoctorsBind req)
        {
            ResultMessage _message;
            try
            {
                var rows = Db.SqlList<int>("EXEC sp_PackagesInDoctors_Insert @PackageId, @DoctorId", new
                {
                    PackageId = req.PackageId,
                    DoctorId = req.DoctorId
                });
                if (rows[0] == 0)
                {
                    _message = new ResultMessage(2, "Связывание", OperationErrors.PackagesDoctorsBindZero);
                    Logger.Log("PackagesDoctors.Bind 0");
                }
                else
                {
                    _message = new ResultMessage(0, "Связывание", OperationResults.PackagesDoctorsBind);
                    Logger.Log("PackagesDoctors.Bind 1");
                }

            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.PackagesDoctorsBind);
                Logger.Log("PackagesDoctors.Bind", e);
                throw;
            }
            return new PackagesDoctorsBindResponse
            {
                Message = _message
            };
        }

        public PackagesDoctorsUnbindResponse Get(PackagesDoctorsUnbind req)
        {
            ResultMessage _message;
            try
            {
                var rows = Db.SqlList<int>("EXEC sp_PackagesInDoctors_Delete @PackageId, @DoctorId", new
                {
                    PackageId = req.PackageId,
                    DoctorId = req.DoctorId
                });
                if (rows[0] == 0)
                {
                    _message = new ResultMessage(2, "Отвязывание", OperationErrors.PackagesDoctorsUnbindZero);
                    Logger.Log("PackagesDoctors.UnBind");
                }
                else
                {
                    _message = new ResultMessage(0, "Отвязывание", OperationResults.PackagesDoctorsUnbind);
                    Logger.Log("PackagesDoctors.UnBind");
                }
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.PackagesDoctorsUnbind);
                Logger.Log("PackagesDoctors.UnBind", e);
                throw;
            }
            return new PackagesDoctorsUnbindResponse
            {
                Message = _message
            };
        }

        #endregion

        #region Nurses and Doctors

        public NursesInDoctorSelectResponse Get(NursesInDoctorSelect req)
        {
            var rows = Db.SqlList<int>("EXEC sp_Doctor_SelectNurses @Id", new
            {
                Id = req.DoctorId
            });

            return new NursesInDoctorSelectResponse { NurseIds = new List<int>(rows) };
        }
        public DoctorsInNurseSelectResponse Get(DoctorsInNurseSelect req)
        {
            var rows = Db.SqlList<int>("EXEC sp_Nurse_SelectDoctors @Id", new
            {
                Id = req.DoctorId
            });

            return new DoctorsInNurseSelectResponse { DoctorIds = new List<int>(rows) };
        }
        public NursesDoctorsBindResponse Get(NursesDoctorsBind req)
        {
            ResultMessage _message;
            try
            {
                var rows = Db.SqlList<int>("EXEC sp_NursesInDoctors_Insert @NurseId, @DoctorId", new
                {
                    NurseId = req.NurseId,
                    DoctorId = req.DoctorId
                });
                if (rows[0] == 0)
                {
                    _message = new ResultMessage(2, "Связывание", OperationErrors.NursesDoctorsBindZero);
                    Logger.Log("NursesDoctors.Bind 0");
                }
                else
                {
                    _message = new ResultMessage(0, "Связывание", OperationResults.NursesDoctorsBind);
                    Logger.Log("NursesDoctors.Bind 1");
                }

            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.NursesDoctorsBind);
                Logger.Log("NursesDoctors.Bind", e);
                throw;
            }
            return new NursesDoctorsBindResponse
            {
                Message = _message
            };
        }

        public NursesDoctorsUnbindResponse Get(NursesDoctorsUnbind req)
        {
            ResultMessage _message;
            try
            {
                var rows = Db.SqlList<int>("EXEC sp_NursesInDoctors_Delete @NurseId, @DoctorId", new
                {
                    NurseId = req.NurseId,
                    DoctorId = req.DoctorId
                });
                if (rows[0] == 0)
                {
                    _message = new ResultMessage(2, "Отвязывание", OperationErrors.NursesDoctorsUnbindZero);
                    Logger.Log("NursesDoctors.UnBind");
                }
                else
                {
                    _message = new ResultMessage(0, "Отвязывание", OperationResults.NursesDoctorsUnbind);
                    Logger.Log("NursesDoctors.UnBind");
                }
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.NursesDoctorsUnbind);
                Logger.Log("NursesDoctors.UnBind", e);
                throw;
            }
            return new NursesDoctorsUnbindResponse
            {
                Message = _message
            };
        }

        #endregion

    }
}
