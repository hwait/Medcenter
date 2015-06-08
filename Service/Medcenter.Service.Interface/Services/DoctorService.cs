namespace Medcenter.Service.Interface.Services
{
    //public class DoctorService : ServiceStack.Service
    //{
    //    readonly Linq2SqlDataContext _db = new Linq2SqlDataContext();
    //    public DoctorSelectResponse Get(DoctorSelect req)
    //    {
    //        var rows = _db.Doctors_select();
    //        var doctors = new List<Doctor>();
    //        foreach (var r in rows)
    //        {
    //            doctors.Add(new Doctor(r.id, r.name, r.col, r.isActive > 0));
    //        }
    //        return new DoctorSelectResponse { Doctors = doctors };
    //    }
    //    public DoctorInsertResponse Post(DoctorInsert req)
    //    {
    //        var did = _db.Doctors_insert(req.Name, req.Color);

    //        return new DoctorInsertResponse { Id = did };
    //    }

    //    public DoctorUpdateResponse Post(DoctorUpdate req)
    //    {
    //        ResponseStatus status = new ResponseStatus();
    //        try
    //        {
    //            _db.Doctors_update(req.Doctor.Id, req.Doctor.Name, req.Doctor.Color);
    //        }
    //        catch (Exception e)
    //        {
    //            status.ErrorCode = "11";
    //            status.Message = e.Message;
    //        }

    //        return new DoctorUpdateResponse { ResponseStatus = status };
    //    }
    //    public DoctorDeleteResponse Get(DoctorDelete req)
    //    {
    //        ResponseStatus status = new ResponseStatus();
    //        try
    //        {
    //            _db.Doctors_delete(req.Id);
    //        }
    //        catch (Exception e)
    //        {
    //            status.ErrorCode = "12";
    //            status.Message = e.Message;
    //        }

    //        return new DoctorDeleteResponse { ResponseStatus = status };
    //    }
    //}
}
