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
    [Route("/sync/main", "POST")]
    public class MainTablesSync : IReturn<MainTablesSyncResponse>
    {
        public int Cid { get; set; }
        public SyncLog OldSyncLog { get; set; }
        public List<SyncStructure> SyncStructures { get; set; }
        public List<SyncRelationsStructure> SyncRelationsStructures { get; set; }
        public List<RemoveItem> SyncRemoveItems { get; set; }
    }

    public class MainTablesSyncResponse
    {
        public List<SyncId> Ids { get; set; }
        public List<SyncStructure> MainStructures { get; set; }
        public List<SyncRelationsStructure> RelationsStructures { get; set; }
        public List<RemoveItem> RemoveStructures { get; set; }
        public int MainGetDur { get; set; }
        public int MainPutDur { get; set; }
        public int RelGetDur { get; set; }
        public int RelPutDur { get; set; }
    }

    [Route("/sync/relations", "POST")]
    public class RelationsSync : IReturn<RelationsSyncResponse>
    {
        public int Cid { get; set; }
        public byte[] Srv { get; set; }
        public List<SyncRelationsStructure> SyncRelationsStructures { get; set; }
    }

    public class RelationsSyncResponse
    {
        public List<SyncRelationsStructure> SyncRelationsStructures { get; set; }
        public int DurGet { get; set; }
        public int DurPut { get; set; }
    }
}
