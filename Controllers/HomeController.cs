using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IS_Control.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace IS_Control.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewData["UserName"]  = User.Identity.Name;
            
            ViewData["UserFullName"] = User.Claims.ToList().
                                        FirstOrDefault(x => x.Type == "UserFullName").Value;
            ViewData["UnitsId"] = User.Claims.ToList().
                                        FirstOrDefault(x => x.Type == "UnitsId").Value;

            ViewData["UserId"] = User.Claims.ToList().
                                        FirstOrDefault(x => x.Type == "UserId").Value;
            ViewData["Role"] = User.Claims.ToList().
                                        FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            /*
            ViewData["reportDt"] =(new DateTime(Convert.ToInt32(User.Claims.ToList().
                                        FirstOrDefault(x => x.Type == "reportDtYear").Value),
                                   Convert.ToInt32(User.Claims.ToList().
                                        FirstOrDefault(x => x.Type == "reportDtMonth").Value), 1)).ToString("MMMM yyyy") ;
            */
            ViewBag.Page = "Home";
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
