using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using IS_Control.Models;
using IS_Control.Tools;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IS_Control.DAL
{
    public static class spDAL
    {
        static string[] mmm = 
        new string[]{
                        "янв","фев","мар",
                        "апр","май","июн",
                        "июл","авг","сен",
                        "окт","ноя","дек"
                    };
        public static string connStr{get;}
        static spDAL()
        {
            var appSettingsJson = AppSettingJSON.GetAppSettings();
            connStr = appSettingsJson["DefaultConnection"];
        }

        public static List<string> AutoCompleteList(string prefix)
        {
            using(SqlConnection _conn = new SqlConnection(spDAL.connStr))
            {
                var r = new List<string>();
                SqlCommand cmd = new SqlCommand("SELECT ProductName as 'label', ProductName as 'val'"+
                                              "FROM VSD "+
                                              "WHERE ProductName like '%"+prefix.Trim()+"%'" +
                                              "GROUP BY ProductName",
                                              _conn);
                _conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    r.Add(dr[0].ToString().Trim());
                _conn.Close();
                return r;
            }
        }
        public static void Update_VSD(VSD tmp)
        {
            using(SqlConnection _conn = new SqlConnection(spDAL.connStr))
            {
                _conn.Update(tmp);
            }
        }
        public static IEnumerable<VSD> GetAll_VSD(string UserID)
        {
            using(SqlConnection _conn = new SqlConnection(spDAL.connStr))
            {
                IEnumerable<VSD> tmpList =
                    _conn.Query<VSD>("SELECT * FROM VSD WHERE userId=@UserIdP", 
                                                new { 
                                                        UserIdP = UserID
                                                    });
                _conn.Close();
                //foreach (var tmp in tmpList){};
                return tmpList;
            }            
        }
        public static VSD GetById_VSD(Guid id)
        {
            using(SqlConnection _conn = new SqlConnection(spDAL.connStr))
            {
                //VSD tmp = _conn.Get<VSD>(id);
                return _conn.Get<VSD>(id);
            }
        }
        public static void Add(VSD newVSD)
        {
            using(SqlConnection _conn = new SqlConnection(spDAL.connStr))
            {
                _conn.Insert(newVSD);
            }
        }
        public static void Delete_VSD(Guid Rid)
        {
            if (Rid == null) return;
            using (SqlConnection _conn = new SqlConnection(spDAL.connStr))
            {
                _conn.Delete(new VSD {id = Rid});
            }
        }

        public static SelectList UnitsList()
        {
            using SqlConnection _conn = new SqlConnection(connStr);
            var tmp = _conn.Query<sp_values>("SELECT ID , [VetUnit] as Text FROM [dbo].[Units]");
            List<sp_values> tL = new List<sp_values>();
            foreach (var tt in tmp) tL.Add(tt);
            return new SelectList(tL, "ID", "Text");
        }

        public static SelectList EdizmList()
        {
            using SqlConnection _conn = new SqlConnection(connStr);
            var tmp = _conn.Query<sp_values>("SELECT id , [Edizm] as Text FROM [dbo].[spEdizm]");
            List<sp_values> tL = new List<sp_values>();
            foreach (var tt in tmp) tL.Add(tt);
            return new SelectList(tL, "ID", "Text");
        }
        public static SelectList TransportList()
        {
            using SqlConnection _conn = new SqlConnection(connStr);
            var tmp = _conn.Query<sp_values>("SELECT id , [Transport] as Text FROM [dbo].[spTransport]");
            List<sp_values> tL = new List<sp_values>();
            foreach (var tt in tmp) tL.Add(tt);
            return new SelectList(tL, "ID", "Text");
        }
        public static SelectList ConclusionList()
        {
            using SqlConnection _conn = new SqlConnection(connStr);
            var tmp = _conn.Query<sp_values>("SELECT id , [Conclusion] as Text FROM [dbo].[spConclusion]");
            List<sp_values> tL = new List<sp_values>();
            foreach (var tt in tmp) tL.Add(tt);
            return new SelectList(tL, "ID", "Text");
        }
        public static string TransportName(string id)
        {
            using (SqlConnection _conn = new SqlConnection(connStr))
            {
                string rez = _conn.QueryFirst<string>(
                    "SELECT TOP 1 [Transport] FROM [spTransport] WHERE [id]=@val", 
                    new { val = id});
                if(rez==null) return ""; else return rez.Trim();
            }
        }
        public static string EdizmName(string id)
        {
            using (SqlConnection _conn = new SqlConnection(connStr))
            {
                string rez = _conn.QueryFirst<string>(
                    "SELECT TOP 1 [Edizm] FROM [spEdizm] WHERE [id]=@val", 
                    new { val = id});
                if(rez==null) return ""; else return rez.Trim();
            }
        }
        public static string ConclusionName(string id)
        {
            using (SqlConnection _conn = new SqlConnection(connStr))
            {
                string rez = _conn.QueryFirst<string>(
                    "SELECT TOP 1 [Conclusion] FROM [spConclusion] WHERE [id]=@val", 
                    new { val = id});
                if(rez==null) return ""; else return rez.Trim();
            }
        }
    }
}

        /*
        public static SelectList ReportToToday()
        {
            List<sp_values> tmpList = new List<sp_values>();
            DateTime dtB = new DateTime(DateTime.Today.Year-1,1,1);
            DateTime dtE =  DateTime.Today;
            while(dtB <= dtE)
            {
                sp_values tmp_sp = new sp_values
                {
                    ID = dtB.ToString("MM yyyy"),//dtB.Month.ToString(),
                    Text = mmm[dtB.Month-1] + " " + dtB.Year.ToString()
                };
                dtB = dtB.AddMonths(1);
                tmpList.Add(tmp_sp);
            }
            SelectList sl = new SelectList(tmpList,"ID","Text");
            string dv = DateTime.Today.ToString("MM yyyy");
            foreach(var it in sl)
                if(it.Value.ToString()== dv) it.Selected = true;
            return sl;//new SelectList(tmpList,"ID","Text");
        } */
