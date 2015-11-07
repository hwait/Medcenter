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
        public int P0 { get; set; }
        [DataMember]
        public int P1 { get; set; }
        [DataMember]
        public int P2 { get; set; }
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
}
