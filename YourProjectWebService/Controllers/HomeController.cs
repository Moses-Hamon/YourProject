using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourProjectWebService.Models;

namespace YourProjectWebService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            var patron = new Patron()
            {
                PatronId = 1,
                PatronName = "Test",
                IsGroup = true
            };


            return View(patron);
        }
    }
}
