using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using Newtonsoft.Json;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Medcenter.Service.Interface
{
    public class Syncronizier : ServiceStack.Service
    {
        private SyncLog _syncLog = new SyncLog();
        private SyncLog _prevSyncLog;
        private SqlConnection _conn;
        private DateTime _dt;
        private JsonServiceClient _cli;
        public void Sync()
        {
            _cli = new JsonServiceClient("http://Nikk-PC/Medcenter.Service.MVC5/api/");
            // 1.1
            var rows = Db.SqlList<SyncLog>("EXEC synco_rv");
            _prevSyncLog = (rows.Count > 0) ? rows[0] : new SyncLog();
            _syncLog.Crv = Db.SqlList<ulong>("EXEC synco_rv")[0];
            
            // 1.2
            _dt = DateTime.Now;
            var syncStructures = Db.SqlList<SyncStructure>("EXEC synco_Main @rv",
                new { rv = _prevSyncLog.Crv });
            _syncLog.ClientMainGetDuration = (DateTime.Now - _dt).Milliseconds;
            _dt = DateTime.Now;

            // 1.3

            var mts = new MainTablesSync { Cid = 1, OldSyncLog = _prevSyncLog, SyncStructures = syncStructures };
            //_syncLog.ClientMainSendBytes = mts.ToJson().Length;

            // 1.4

            var r = _cli.Post(mts);
            _syncLog.AllMainDuration = (DateTime.Now - _dt).Milliseconds;
            //_syncLog.ClientMainGetBytes = r.ToJson().Length;
            
            _syncLog.ServerMainGetDuration = r.DurGet;
            _syncLog.ServerMainPutDuration = r.DurPut;
            _syncLog.Srv = r.Srv;
            //_cli.PostAsync(mts).Success(r =>
            //        {
            //            // 3.1
            //            //_syncLog.AllMainDuration = (DateTime.Now - _dt).Milliseconds;
            //            //_syncLog.ClientMainGetBytes = r.ToJson().Length;
            //            //_syncLog.ServerMainGetDuration = r.DurGet;
            //            //_syncLog.ServerMainPutDuration = r.DurPut;
            //            //_syncLog.Srv = r.Srv;
            //            //MainTablesReceived(r.Srv, r.Ids, r.MainStructures);
            //        })
            //        .Error(ex =>
            //        {
            //            throw ex;
            //        });
        }
    }
}
