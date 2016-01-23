using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace DBUtils
{
    class Program
    {
        static private SqlConnection _connC, _connS;
        static void Main(string[] args)
        {
            

            _connC = new SqlConnection(@"Data Source=NIKK-PC\SQLEXPRESS;Initial Catalog=Medcenter;Integrated Security=True");
            _connS = new SqlConnection(@"Data Source=NIKK-PC\SQLEXPRESS;Initial Catalog=MedSrv;Integrated Security=True");
            _connC.Open();
            _connS.Open();

            CompareTables("Cities", 0);
            CompareTables("Discounts", 1);

            CompareTables("DiscountsInPackages", 0, false);
            CompareTables("DoctorInspectionParaphrases", 0, false);
            CompareTables("DoctorPatterns", 0, false);
            CompareTables("Doctors", 0);
            CompareTables("Headers", 1);
            CompareTables("InspectionsInPackages", 0, false);
            CompareTables("NursesInDoctors", 0, false);
            CompareTables("PackageGroups", 0);
            CompareTables("Packages", 1);
            CompareTables("PackagesInDoctors", 0, false);
            CompareTables("PackagesInGroups", 0, false);
            CompareTables("PackagesInReception", 0, false);
            CompareTables("Paraphrases", 0);
            CompareTables("Patients", 0);
            CompareTables("PatternPositions", 1, false);
            CompareTables("Patterns", 1);
            CompareTables("Payments", 1);
            CompareTables("Phrases", 1);
            CompareTables("Positions", 1);
            CompareTables("Receptions", 1);
            CompareTables("Schedules", 1);
            CompareTables("SurveyPhrases", 0, false);
            CompareTables("Surveys", 1);
            CompareTables("UserAuth", 0);

            _connC.Close();
            _connS.Close();
            Console.ReadLine();
        }

        private static void CompareTables(string tableName, int cid, bool isMain=true)
        {
            Console.WriteLine();
            Console.WriteLine("Compare {0}:", tableName);
            if(!isMain) Console.Write("(rows number only).");
            SqlCommand cmdS, cmdC;
            SqlDataReader rdrC, rdrS;
            int cn = 0, sn = 0;
            string fn = "", valC, valS;
            string sqlCliRowsNmb = string.Format("select count(*) from {0}", tableName);
            string sqlSrvRowsNmb = (cid == 0) ? sqlCliRowsNmb : string.Format("select count(*) from {0} where Cid={1}", tableName, cid);
            string sqlS = "";
            cmdC = new SqlCommand(sqlCliRowsNmb, _connC);
            using (rdrC = cmdC.ExecuteReader())
            {
                if (rdrC.Read())
                {
                    cn = (int)rdrC[0];
                }
            }
            cmdS = new SqlCommand(sqlSrvRowsNmb, _connS);
            using (rdrS = cmdS.ExecuteReader())
            {
                if (rdrS.Read())
                {
                    sn = (int)rdrS[0];
                }
            }
            bool hasError = false;

            if (cn != sn)
            {
                Console.WriteLine("Rows number is different! Client: {0}, Server={1}", cn, sn);
                hasError = true;
            }
            if (isMain)
            {
                cmdC = new SqlCommand(string.Format("select * from  {0}", tableName), _connC);

                using (rdrC = cmdC.ExecuteReader())
                {
                    while (rdrC.Read())
                    {
                        cmdS = new SqlCommand(string.Format("select * from {0} where Id={1}", tableName, rdrC["Sid"]),
                            _connS);
                        using (rdrS = cmdS.ExecuteReader())
                        {
                            if (rdrS.Read())
                            {
                                for (int i = 0; i < rdrS.FieldCount; i++)
                                {
                                    fn = rdrS.GetName(i);
                                    if (fn != "Id" && fn != "rv" && fn != "Cid" && !fn.Contains("Id"))
                                    {
                                        
                                        valC = rdrC[fn].ToString();
                                        valS = rdrS[fn].ToString();
                                        if (valC != valS)
                                        {
                                            Console.WriteLine("Diff Id={0}, Sid={1}: C[{2}] {3} != S[{2}] {4}",
                                                rdrC["Id"],
                                                rdrC["Sid"], fn, valC, valS);
                                            hasError = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if(hasError) Console.WriteLine("done with errors!");
            else Console.WriteLine("done {0} rows!",cn);
        }
    }
}
