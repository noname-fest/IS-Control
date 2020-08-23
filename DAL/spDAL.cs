using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
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

        public static SelectList UnitsList()
        {
            using SqlConnection _conn = new SqlConnection(connStr);
            var tmp = _conn.Query<sp_values>("SELECT ID , [VetUnit] as Text FROM [dbo].[Units]");
            List<sp_values> tL = new List<sp_values>();
            foreach (var tt in tmp) tL.Add(tt);
            return new SelectList(tL, "ID", "Text");
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
