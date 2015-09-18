using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using ServiceStack.OrmLite;

namespace Medcenter.Service.Interface.Services
{
    public class PatientService : ServiceStack.Service
    {

        #region Patient

        public PatientsSelectResponse Get(PatientsSelect req)
        {
            var rows = Db.SqlList<Patient>("EXEC sp_Patients_Select_BySurname @Text", new {Text = req.Text});
            return new PatientsSelectResponse {Patients = new List<Patient>(rows)};
        }

        public PatientSelectResponse Get(PatientSelect req)
        {
            var row = Db.Single<Patient>("EXEC sp_Patients_Select_BySurname  @Id", new { Id = req.Id});
            return new PatientSelectResponse {Patient = row};
        }

        public PatientSaveResponse Post(PatientSave req)
        {
            int id = 0;
            ResultMessage _message;
            if (req.Patient.Id > 0) // Package exists so we're saving 
            {
                try
                {
                    id =
                        Db.Single<int>(
                            "EXEC sp_Patients_Update @Id, @Surname, @FirstName, @SecondName, @BirthDate, @Gender, @Address, @PhoneNumber, @MobileCode, @MobileNumber, @Email, @CityId",
                            new
                            {
                                Id = req.Patient.Id,
                                Surname = req.Patient.Surname,
                                FirstName = req.Patient.FirstName,
                                SecondName = req.Patient.SecondName,
                                BirthDate = req.Patient.BirthDate,
                                Gender = req.Patient.Gender,
                                Address = req.Patient.Address,
                                PhoneNumber = req.Patient.PhoneNumber,
                                MobileCode = req.Patient.MobileCode,
                                MobileNumber = req.Patient.MobileNumber,
                                Email = req.Patient.Email,
                                CityId = req.Patient.City.Id
                            });
                    _message = new ResultMessage(0, "Сохранение исследования", OperationResults.PatientSave);
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.PatientSave);
                    Logger.Log("PatientSaveResponse.Saving ", e);
                    throw;
                }
            }
            else //New Patient
            {
                try
                {
                    id =
                        Db.Single<int>(
                            "EXEC sp_Patients_Insert @Surname, @FirstName, @SecondName, @BirthDate, @Gender, @Address, @PhoneNumber, @MobileCode, @MobileNumber, @Email, @CityId",
                            new
                            {
                                Surname = req.Patient.Surname,
                                FirstName = req.Patient.FirstName,
                                SecondName = req.Patient.SecondName,
                                BirthDate = req.Patient.BirthDate,
                                Gender = req.Patient.Gender,
                                Address = req.Patient.Address,
                                PhoneNumber = req.Patient.PhoneNumber,
                                MobileCode = req.Patient.MobileCode,
                                MobileNumber = req.Patient.MobileNumber,
                                Email = req.Patient.Email,
                                CityId = req.Patient.City.Id
                            });
                    _message = new ResultMessage(0, "Новое исследование", OperationResults.PatientCreate);
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.PatientCreate);
                    Logger.Log("PatientSaveResponse.NewPackage", e);
                    throw;
                }
            }

            return new PatientSaveResponse
            {
                PatientId = id,
                Message = _message
            };
        }
        public PatientClarifyResponse Post(PatientClarify req)
        {
            int id = 0;
            ResultMessage _message;
            try
            {
                id =
                    Db.Single<int>(
                        "EXEC sp_Patients_Clarify @Id, @Surname, @FirstName, @SecondName, @BirthDate",
                        new
                        {
                            Id = req.Patient.Id,
                            Surname = req.Patient.Surname,
                            FirstName = req.Patient.FirstName,
                            SecondName = req.Patient.SecondName,
                            BirthDate = req.Patient.BirthDate
                        });
                _message = new ResultMessage(0, "Уточнение данных пациента", OperationResults.PatientSave);
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.PatientSave);
                Logger.Log("PatientClarifyResponse.Saving ", e);
                throw;
            }
            return new PatientClarifyResponse
            {
                Message = _message
            };
        }
        public PatientDeleteResponse Get(PatientDelete req)
        {
            ResultMessage _message;
            try
            {
                var res = Db.SqlList<int>("EXEC sp_Patients_Delete @Id", new
                {
                    Id = req.PatientId
                });
                _message = new ResultMessage(0, "Инспекции:", OperationResults.PatientDelete);
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.PatientDelete);
                Logger.Log("PatientDeleteResponse", e);
                throw;
            }
            return new PatientDeleteResponse {Message = _message};
        }

        #endregion

        #region City

        public CitiesSelectResponse Get(CitiesSelect req)
        {
            var rows = Db.SqlList<City>("EXEC sp_Cities_Select");
            return new CitiesSelectResponse { Cities = new List<City>(rows) };
        }
        public CitySaveResponse Post(CitySave req)
        {
            int id = 0;
            ResultMessage _message;
            if (req.City.Id > 0) // Package exists so we're saving 
            {
                try
                {
                    id =
                        Db.Single<int>(
                            "EXEC sp_Cities_Update @Id, @Name, @PhoneCode",
                            new
                            {
                                Id = req.City.Id,
                                Name = req.City.Name,
                                PhoneCode = req.City.PhoneCode
                            });
                    _message = new ResultMessage(0, "Сохранение исследования", OperationResults.CitySave);
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.CitySave);
                    Logger.Log("CitySaveResponse.Saving ", e);
                    throw;
                }
            }
            else //New City
            {
                try
                {
                    id =
                        Db.Single<int>(
                            "EXEC sp_Cities_Insert @Name, @PhoneCode",
                            new
                            {
                                Name = req.City.Name,
                                PhoneCode = req.City.PhoneCode
                            });
                    _message = new ResultMessage(0, "Новое исследование", OperationResults.CityCreate);
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.CityCreate);
                    Logger.Log("CitySaveResponse.NewPackage", e);
                    throw;
                }
            }

            return new CitySaveResponse
            {
                CityId = id,
                Message = _message
            };
        }

        public CityDeleteResponse Get(CityDelete req)
        {
            ResultMessage _message;
            try
            {
                var res = Db.SqlList<int>("EXEC sp_Cities_Delete @Id", new
                {
                    Id = req.CityId
                });
                _message = new ResultMessage(0, "Инспекции:", OperationResults.CityDelete);
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.CityDelete);
                Logger.Log("CityDeleteResponse", e);
                throw;
            }
            return new CityDeleteResponse { Message = _message };
        }

        #endregion
    }
}
