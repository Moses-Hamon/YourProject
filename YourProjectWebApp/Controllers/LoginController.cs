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
        /// <summary>
        /// Entry point for login screen
        /// </summary>
        /// <returns></returns>
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Validates a user using webservice
        /// </summary>
        /// <param name="staff">Model containing form data</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(Staff staff)
        {
            // creates connection
            var svc = new YourProjectServiceSoapClient();
            // validates the username and password with web service
            var results = svc.ValidCredentials(staff);
            if (results)
            {
                // Authenticates the user and redirects to Home screen
                FormsAuthentication.RedirectFromLoginPage(staff.UserName, true);
                Session.Add("User", staff.UserName);
                return View();

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Incorrect Username or Password, Please try again!");
                return View(staff);
            }

        }
        /// <summary>
        /// Logs out a user
        /// </summary>
        /// <returns>Login Screen</returns>
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