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

        #region Doctor

        public DoctorsSelectResponse Get(DoctorsSelect req)
        {
            var rows = Db.SqlList<Doctor>("EXEC sp_Doctors_Select");
            foreach (var r in rows)
                r.InspectionIds = Db.SqlList<int>("EXEC sp_Doctor_SelectInspections @DoctorId", new { DoctorId = r.Id });
            return new DoctorsSelectResponse { Doctors = new List<Doctor>(rows) };
        }

        public DoctorSaveResponse Post(DoctorSave req)
        {
            int id = 0;
            ResultMessage _message;
            if (req.Doctor.Id > 0) // Inspection exists so we're saving 
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
                    Logger.Log("InspectionSaveResponse.NewInspection");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.DoctorCreate);
                    Logger.Log("InspectionSaveResponse.NewInspection", e);
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

        #region Inspections and Doctors

        public InspectionsInDoctorSelectResponse Get(InspectionsInDoctorSelect req)
        {
            var rows = Db.SqlList<int>("EXEC sp_Doctor_SelectInspections @Id", new
            {
                Id = req.DoctorId
            });

            return new InspectionsInDoctorSelectResponse { InspectionIds = new List<int>(rows) };
        }
        public DoctorsInInspectionSelectResponse Get(DoctorsInInspectionSelect req)
        {
            var rows = Db.SqlList<int>("EXEC sp_Inspection_SelectDoctors @Id", new
            {
                Id = req.DoctorId
            });

            return new DoctorsInInspectionSelectResponse { DoctorIds = new List<int>(rows) };
        }
        public InspectionsDoctorsBindResponse Get(InspectionsDoctorsBind req)
        {
            ResultMessage _message;
            try
            {
                var rows = Db.SqlList<int>("EXEC sp_InspectionsInDoctors_Insert @InspectionId, @DoctorId", new
                {
                    InspectionId = req.InspectionId,
                    DoctorId = req.DoctorId
                });
                if (rows[0] == 0)
                {
                    _message = new ResultMessage(2, "Связывание", OperationErrors.InspectionsDoctorsBindZero);
                    Logger.Log("InspectionsDoctors.Bind 0");
                }
                else
                {
                    _message = new ResultMessage(0, "Связывание", OperationResults.InspectionsDoctorsBind);
                    Logger.Log("InspectionsDoctors.Bind 1");
                }

            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.InspectionsDoctorsBind);
                Logger.Log("InspectionsDoctors.Bind", e);
                throw;
            }
            return new InspectionsDoctorsBindResponse
            {
                Message = _message
            };
        }

        public InspectionsDoctorsUnbindResponse Get(InspectionsDoctorsUnbind req)
        {
            ResultMessage _message;
            try
            {
                var rows = Db.SqlList<int>("EXEC sp_InspectionsInDoctors_Delete @InspectionId, @DoctorId", new
                {
                    InspectionId = req.InspectionId,
                    DoctorId = req.DoctorId
                });
                if (rows[0] == 0)
                {
                    _message = new ResultMessage(2, "Отвязывание", OperationErrors.InspectionsDoctorsUnbindZero);
                    Logger.Log("InspectionsDoctors.UnBind");
                }
                else
                {
                    _message = new ResultMessage(0, "Отвязывание", OperationResults.InspectionsDoctorsUnbind);
                    Logger.Log("InspectionsDoctors.UnBind");
                }
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.InspectionsDoctorsUnbind);
                Logger.Log("InspectionsDoctors.UnBind", e);
                throw;
            }
            return new InspectionsDoctorsUnbindResponse
            {
                Message = _message
            };
        }

        #endregion

    }
}
