using Microsoft.AspNetCore.Mvc;
using IS_Control.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using IS_Control.DAL;
using System.Linq;

namespace IS_Control.Controllers
{
    public class VSDController : Controller
        {
            [Authorize]
            public IActionResult Index()
            {
                string UserId = User.Claims.ToList().
                        FirstOrDefault(x => x.Type == "UserId").Value;

                ViewBag.Page = "Index VSD";
                return View(spDAL.GetAll_VSD(UserId).ToList());
            }
            [Authorize]
            [HttpGet]
            public IActionResult Create()
            {
                string CurrentUserId  = 
                    User.Claims.ToList().FirstOrDefault(x => x.Type == "UserId").Value;
                string CurrentUnitsId = 
                    User.Claims.ToList().FirstOrDefault(x => x.Type == "UnitsId").Value;
                ViewBag.UnitsList = spDAL.UnitsList();
                ViewBag.EdizmList = spDAL.EdizmList();
                ViewBag.TransportList = spDAL.TransportList();
                ViewBag.ConclusionList = spDAL.ConclusionList();
                VSD tmp = new VSD()
                {
                    userId = CurrentUserId
                };
                ViewBag.Page = "VSD";
                return View(tmp);
            }
            [Authorize]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create([Bind] VSD tmp)
            {
                if (ModelState.IsValid)
                {
                    spDAL.Add(tmp);
                    //проверка на существования аналогичной записи
                    //if (BioPrepDAL.IsUniqueRecord(tmp))
                    //    BioPrepDAL.Add_BioPrep(tmp);
                    //else
                    //{
                        //TempData["EM"] = "Такая запись уже существует";
                        //return LocalRedirect("~/Home/Error");
                        //return RedirectToAction("Error");
                    //};
                    return RedirectToAction("Index");
                }
                ViewBag.Page = "VSD";
                return View(tmp);
            }


        }
}