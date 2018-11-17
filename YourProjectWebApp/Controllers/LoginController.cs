using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YourProjectWebApp.WebServiceYourProject;

namespace YourProjectWebApp.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Staff staff)
        {
            var svc = new YourProjectServiceSoapClient();
            var results = svc.ValidCredentials(staff);
            if (results)
            {
                FormsAuthentication.RedirectFromLoginPage(staff.UserName, true);
                Session.Add("User", staff.UserName);
                return View();
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Credentials received: {staff.UserName} + {staff.UserPassword}");
                return View(staff);
            }
            
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}