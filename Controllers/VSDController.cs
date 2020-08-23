using Microsoft.AspNetCore.Mvc;
using IS_Control.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace IS_Control.Controllers
{
    public class VSDController : Controller
        {
            [Authorize]
            public IActionResult Index()
            {
                List<VSD> list = new List<VSD>();
                return View(list);
            } 
           //public static string connStr {get;}
            //var appSettingsJson = AppSettingJSON.GetAppSettings();
            //connStr = appSettingsJson["DefaultConnection"];
        }
}