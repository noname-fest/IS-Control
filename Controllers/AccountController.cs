using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Dapper;
using Dapper.Contrib;
using IS_Control.Models;
using System.Data.SqlClient;
using IS_Control.Tools;
using Microsoft.AspNetCore.Authentication;

namespace IS_Control.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login() 
        {
            ViewBag.Page = "Home";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if(loginModel != null)
            if (ModelState.IsValid)
            {
                var appSettingsJson = AppSettingJSON.GetAppSettings();
                var connectionString = appSettingsJson["DefaultConnection"];

                using(SqlConnection _conn = new SqlConnection(connectionString))
                {
                    string q = "SELECT * FROM Users WHERE username=@usr_name and userpassword=@usr_pwd";
                    var param = new
                        {
                          usr_name = loginModel.username,
                          usr_pwd  = loginModel.userpassword
                        };
                    var user = _conn.QueryFirstOrDefault<User>(q,param);
                  if (user != null)
                    {
                       await Authenticate(loginModel.username).ConfigureAwait(false);
                      return RedirectToAction("Index", "Home");
                    }
                  else
                    {
                     ModelState.AddModelError("", "Некорректные логин или пароль");
                    }
                }
            }
            ViewBag.Page = "Home";
            return View(loginModel);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public  IActionResult UsersEdit()
        {
            var appSettingsJson = AppSettingJSON.GetAppSettings();
            var connectionString = appSettingsJson["DefaultConnection"];

            using(SqlConnection _conn = new SqlConnection(connectionString))
            {
                var tmpList = _conn.Query<User>("SELECT * FROM Users");
                _conn.Close();
                ViewBag.Page = "Home";
                //ViewBag.DtList = spDAL.ReportToToday();
                return View(tmpList);
            };
        }

        /*
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ReportDtToAllUsers(DateTime dt)
        {
            var appSettingsJson = AppSettingJSON.GetAppSettings();
            var connectionString = appSettingsJson["DefaultConnection"];

            using(SqlConnection _conn = new SqlConnection(connectionString))
            {
                string q = "UPDATE Users SET ReportDt=@dd WHERE [Role]='Client'";
                var param = new {dd = dt};
                _conn.Execute(q,param);
                _conn.Close();
            }
            //ViewBag.Page = "Home";
            //ViewBag.DtList = spDAL.ReportToToday();
            return RedirectToAction("UsersEdit");
        }
        */
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var appSettingsJson = AppSettingJSON.GetAppSettings();
            var connectionString = appSettingsJson["DefaultConnection"];

            using(SqlConnection _conn = new SqlConnection(connectionString))
            {
                var usr = _conn.QueryFirst<User>("SELECT * FROM Users WHERE Id=@idd", new {idd = id});
                if(usr == null) return NotFound();
                //ViewBag.KIDroList = spDAL.KIDroList();
                ViewBag.Page = "Home";
                return View(usr);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]  
        public IActionResult Edit([Bind] User objUsr)
        {
            if(objUsr==null) return NotFound();
            if (ModelState.IsValid)
            {
                //if(BioPrepDAL.IsUniqueRecord(objBioPrep))
                    //BioPrepDAL.Update_BioPrep(objBioPrep);
                var appSettingsJson = AppSettingJSON.GetAppSettings();
                var connectionString = appSettingsJson["DefaultConnection"];

                using(var _conn = new SqlConnection(connectionString))
                    {
                        _conn.Execute("UPDATE Users SET KIDro=@_kidro,"+
                        "username=@_username, UserFullname=@_UserFullname,"+
                        "userpassword=@_userpassword, Role=@_Role, reportDt=@_reportDt "+
                        "WHERE Id=@_Id",
                        new {
                            _Id = objUsr.Id,
                            _kidro = objUsr.KIDro,
                            _username = objUsr.username,
                            _UserFullname = objUsr.UserFullname,
                            _userpassword = objUsr.userpassword,
                            _Role = objUsr.Role,
                            _reportDt = objUsr.reportDt
                        } );   
                        return RedirectToAction("UsersEdit");
                    }
            }
            ViewBag.Page = "Home";
            return View(objUsr);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int Id)
        {
            var appSettingsJson = AppSettingJSON.GetAppSettings();
            var connectionString = appSettingsJson["DefaultConnection"];

            using(SqlConnection _conn = new SqlConnection(connectionString))
            {
                var usr = _conn.QueryFirst<User>("SELECT * FROM Users WHERE Id=@idd", new {idd = Id});
                if(usr == null) return NotFound();
                ViewBag.Page = "Home";
                return View(usr);
            }
        }


        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int Id)
        {
            //BioPrepDAL.Delete_BioPrep(id);
            var appSettingsJson = AppSettingJSON.GetAppSettings();
            var connectionString = appSettingsJson["DefaultConnection"];

            using(SqlConnection _conn = new SqlConnection(connectionString))
                _conn.Execute("DELETE FROM Users WHERE Id=@idd", new {idd = Id});
            ViewBag.Page = "Home";
            return RedirectToAction("UsersEdit");
        }        


        [Authorize(Roles = "Admin")]
        public IActionResult Register()
            {
                //ViewBag.KIDroList = spDAL.KIDroList();
                ViewBag.Page = "Home";
                return View();
            }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (registerModel != null)
            if (ModelState.IsValid)
            {
                var appSettingsJson = AppSettingJSON.GetAppSettings();
                var connectionString = appSettingsJson["DefaultConnection"];

                SqlConnection _conn = new SqlConnection(connectionString);
                int usr_count = await _conn.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM Users WHERE username=@usr_name",
                                                    new{usr_name = registerModel.username}).ConfigureAwait(false);
                if (usr_count == 0)
                {
                        var param = new
                        {
                            UsrFN = registerModel.UserFullname,
                            usrname = registerModel.username,
                            usrpwd = registerModel.userpassword,
                            @idro = registerModel.KIDro,
                            @rr = registerModel.Role,
                            @rDt = registerModel.reportDt
                        };
                        _conn.Execute("INSERT INTO Users (UserFullname,username,userpassword,KIDro,Role,reportDt) "+
                            "VALUES (@UsrFN,@usrname,@usrpwd,@idro,@rr,@rDt)" ,
                            param);
                    _conn.Close();
                    return RedirectToAction("Index", "Home");
                } else
                {
                    ModelState.AddModelError("", "Некорректные логин или пароль");
                }
                _conn.Close();
            }
            ViewBag.Page = "Home";
            return View(registerModel);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
            return RedirectToAction("Login", "Account");
        }

        private async Task Authenticate(string username)
        {
            var appSettingsJson = AppSettingJSON.GetAppSettings();
            var connectionString = appSettingsJson["DefaultConnection"];

            using(SqlConnection _conn = new SqlConnection(connectionString))
            {
                User usr = _conn.QueryFirstOrDefault<User>("SELECT * FROM Users WHERE username=@usr_name",
                                                            new{usr_name = username});
                string usrFullName;
                string roleP;
                string KIDro;
                DateTime rDt; 

                if(usr.Role!=null) roleP = usr.Role; else roleP ="Client";
                if(usr.KIDro!=null) KIDro =usr.KIDro; else KIDro = "";
                if(usr.reportDt!=null) rDt = usr.reportDt; 
                    else rDt = new DateTime(DateTime.Today.Year,DateTime.Today.Month-1,1);
                if(usr.UserFullname!=null) usrFullName = usr.UserFullname; else usrFullName = "";

                int Y = rDt.Year;
                int M = rDt.Month;
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, roleP),
                    new Claim("KIDro", KIDro),
                    new Claim("Role", roleP),
                    new Claim("reportDtYear", Y.ToString()),
                    new Claim("reportDtMonth", M.ToString()),
                    new Claim("UserFullName", usrFullName)
                };
                //CultureInfo.CurrentCulture = new CultureInfo("ky-KG");
                //CultureInfo.CurrentUICulture = new CultureInfo("ky");

                //Response.Cookies.Append(
                //CookieRequestCultureProvider.DefaultCookieName,
                //CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("ky")),
                //new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                //);


                var id = new ClaimsIdentity(claims, "ApplicationCookie",
                    ClaimsIdentity.DefaultNameClaimType, 
                    ClaimsIdentity.DefaultRoleClaimType);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                new ClaimsPrincipal(id)).ConfigureAwait(false);
                }
        }
     }
}
