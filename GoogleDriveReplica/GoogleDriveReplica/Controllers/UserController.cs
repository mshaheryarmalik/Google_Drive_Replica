using Drive.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoogleDriveReplica.Controllers
{
    public class UserController : Controller
    {
        [HttpPost]
        public ActionResult Login()
        {
            String login = Request["login"];
            String password = Request["password"];
            UserDTO obj = (UserDTO) Drive.BAL.UserBO.ValidateUser(login,password);
            if (obj != null)
            {
                Session["uname"] = login;
                Session["name"] = obj.Name;
                Session["uid"] = obj.Id;
                return RedirectToAction("Home","User");
            }
            TempData["Message"] = "Invalid Login/Password";
            return RedirectToAction("Login", "MainScreen");
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Logout()
        {
            if(Session["uname"] != null)
                Session["uname"] = null;
            return Redirect("~/MainScreen/Login");
        }
	}
}