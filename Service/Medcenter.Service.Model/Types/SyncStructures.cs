using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class SyncStructure
    {
        [DataMember]
        public int TableType { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Sid { get; set; }
        [DataMember]
        public string P0 { get; set; }
        [DataMember]
        public string P1 { get; set; }
        [DataMember]
        public string P2 { get; set; }
        [DataMember]
        public string P3 { get; set; }
        [DataMember]
        public string P4 { get; set; }
        [DataMember]
        public string P5 { get; set; }
        [DataMember]
        public string P6 { get; set; }
        [DataMember]
        public string P7 { get; set; }
        [DataMember]
        public string P8 { get; set; }
        [DataMember]
        public string P9 { get; set; }
        [DataMember]
        public string P10 { get; set; }
        [DataMember]
        public string P11 { get; set; }
        [DataMember]
        public string P12 { get; set; }
        [DataMember]
        public string P13 { get; set; }
        [DataMember]
        public string P14 { get; set; }
        [DataMember]
        public string P15 { get; set; }

        public SyncStructure()
        {
            
        }

        public SyncStructure(SqlDataReader row)
        {
            TableType = (int) row["tt"];
            Id = (int) row["Id"];
            int sid=0;
            int.TryParse(row["Sid"].ToString(), out sid);
            Sid = sid;
            P0 = row["p0"].ToString();
            P1 = row["p1"].ToString();
            P2 = row["p2"].ToString();
            P3 = row["p3"].ToString();
            P4 = row["p4"].ToString();
            P5 = row["p5"].ToString();
            P6 = row["p6"].ToString();
            P7 = row["p7"].ToString();
            P8 = row["p8"].ToString();
            P9 = row["p9"].ToString();
            P10 = row["p10"].ToString();
            P11 = row["p11"].ToString();
            P12 = row["p12"].ToString();
            P13 = row["p13"].ToString();
            P14 = row["p14"].ToString();
            P15 = row["p15"].ToString();
        }
    }
    [DataContract]
    public class SyncRelationsStructure
    {
        [DataMember]
        public int TableType { get; set; }
        [DataMember]
        public int Id0 { get; set; }
        [DataMember]
        public int Id1 { get; set; }
        [DataMember]
        public int Id2 { get; set; }
        [DataMember]
        public string P0 { get; set; }
        [DataMember]
        public string P1 { get; set; }
        [DataMember]
        public string P2 { get; set; }
        public SyncRelationsStructure()
        {

        }

        public SyncRelationsStructure(SqlDataReader row)
        {
            TableType = (int)row["tt"];
            int id0,id1,id2;
            int.TryParse(row["id0"].ToString(), out id0);
            int.TryParse(row["id1"].ToString(), out id1);
            int.TryParse(row["id2"].ToString(), out id2);
            Id0 = id0;
            Id1 = id1;
            Id2 = id2;
            P0 = row["p0"].ToString();
            P1 = row["p1"].ToString();
            P2 = row["p2"].ToString();
        }
    }
    [DataContract]
    public class SyncId
    {
        [DataMember]
        public int TableType { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Sid { get; set; }

        public SyncId()
        {

        }

        public SyncId(SyncStructure row, int sid)
        {
            TableType = row.TableType;
            Id = row.Id;
            Sid = sid;
        }
    }
    [DataContract]
    public class SyncLog
    {
        [DataMember]
        public ulong Crv { get; set; }
        [DataMember]
        public byte[] Srv { get; set; }
        [DataMember]
        public int ClientMainGetDuration { get; set; }
        [DataMember]
        public int ClientRelationsGetDuration { get; set; }
        [DataMember]
        public int ClientMainIdsUpdateDuration { get; set; }
        [DataMember]
        public int ClientMainPutDuration { get; set; }
        [DataMember]
        public int ClientRelationsPutDuration { get; set; }
        [DataMember]
        public int ServerMainGetDuration { get; set; }
        [DataMember]
        public int ServerRelationsGetDuration { get; set; }
        [DataMember]
        public int ServerMainPutDuration { get; set; }
        [DataMember]
        public int ServerRelationsPutDuration { get; set; }
        [DataMember]
        public int AllMainDuration { get; set; }
        [DataMember]
        public int AllRelationsDuration { get; set; }
        [DataMember]
        public int ClientMainSendBytes { get; set; }
        [DataMember]
        public int ClientMainGetBytes { get; set; }
        [DataMember]
        public int ClientRelationsSendBytes { get; set; }
        [DataMember]
        public int ClientRelationsGetBytes { get; set; }

        public SyncLog(SqlDataReader row)
        {
            Crv = (ulong) row["crv"];
            Srv = (byte[]) row["srv"];
            ClientMainGetDuration = (int)row["ClientMainGetDuration"];
            ClientRelationsGetDuration = (int)row["ClientRelationsGetDuration"]; ;
            ClientMainIdsUpdateDuration = (int)row["ClientMainIdsUpdateDuration"]; ;
            ClientMainPutDuration = (int)row["ClientMainPutDuration"]; ;
            ClientRelationsPutDuration = (int)row["ClientRelationsPutDuration"]; ;
            ServerMainGetDuration = (int)row["ServerMainGetDuration"]; ;
            ServerRelationsGetDuration = (int)row["ServerRelationsGetDuration"]; ;
            ServerMainPutDuration = (int)row["ServerMainPutDuration"]; ;
            ServerRelationsPutDuration = (int)row["ServerRelationsPutDuration"]; ;
            AllMainDuration = (int)row["AllMainDuration"]; ;
            AllRelationsDuration = (int)row["AllRelationsDuration"]; ;
            ClientMainSendBytes = (int)row["ClientMainSendBytes"]; ;
            ClientMainGetBytes = (int)row["ClientMainGetBytes"]; ;
            ClientRelationsSendBytes = (int)row["ClientRelationsSendBytes"]; ;
            ClientRelationsGetBytes = (int)row["ClientRelationsGetBytes"]; ;
        }
        public SyncLog()
        {
            //Crv=new byte[8];
            Srv = new byte[8];
        }
    }
    [DataContract]
    public class RemoveItem
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public byte TableNameId { get; set; }
        [DataMember]
        public int Sid0 { get; set; }
        [DataMember]
        public int Sid1 { get; set; }
        [DataMember]
        public int Sid2 { get; set; }

        public RemoveItem (SqlDataReader row)
        {
            Id = (int)row["Id"];
            TableNameId = (byte)row["TableNameId"]; ;
            Sid0 = (int)row["Sid0"]; ;
            Sid1 = (int)row["Sid1"]; ;
            Sid2 = (int)row["Sid2"]; ;
        }
        public RemoveItem()
        {
        }
    }
}
