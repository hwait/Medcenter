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
        public List<SyncStructure> SyncStructures { get; set; }
    }

    public class MainTablesSyncResponse
    {
        public List<SyncId> Ids { get; set; }
    }

    [Route("/sync/relations", "POST")]
    public class RelationsSync : IReturn<RelationsSyncResponse>
    {
        public int Cid { get; set; }
        public byte[] Rv { get; set; }
        public List<SyncRelationsStructure> SyncStructures { get; set; }
    }

    public class RelationsSyncResponse
    {
        public byte[] Rv { get; set; }
        public List<SyncStructure> SyncStructures { get; set; }
        public List<SyncRelationsStructure> SyncRelationsStructures { get; set; }
    }
}
