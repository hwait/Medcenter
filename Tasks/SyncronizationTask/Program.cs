using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Types;
using ServiceStack;

namespace SyncronizationTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var cli = new JsonServiceClient("http://Nikk-PC/Medcenter.Service.MVC5/api/");
            List<SyncStructure> syncStructures=new List<SyncStructure>();
            var fn = "rv.txt";
            byte[] rv = (File.Exists(fn)) ? File.ReadAllBytes(fn) : new byte[8];
            
            using (SqlConnection conn = new SqlConnection(@"Data Source=NIKK-PC\SQLEXPRESS;Initial Catalog=Medcenter;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("synco_Main", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@rv", rv));
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        syncStructures.Add(new SyncStructure(rdr));
                    }
                }
                SqlCommand cmd1 = new SqlCommand("select @@DBTS as rv", conn);
                cmd1.CommandType = CommandType.Text;
                using (SqlDataReader rdr = cmd1.ExecuteReader())
                {   
                    rdr.Read();
                    File.WriteAllBytes(fn, (byte[])rdr["rv"]);
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(fn, false))
                    //{
                    //    file.Write(GetString((byte[])rdr["rv"]));
                    //    file.Close();
                    //}
                }
                conn.Close();
            }

        }
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[8];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, 8);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

    }
}
