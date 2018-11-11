using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using YourProjectWebService.Models;


namespace YourProjectWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetMembers()
        {
            IEnumerable<Tool> tools = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:59636/api/tool/");

                var responseTask = client.GetAsync("tool");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IEnumerable<Tool>>();
                    readTask.Wait();

                    tools = readTask.Result;
                }
                else
                {
                    tools = Enumerable.Empty<Tool>();
                    ModelState.AddModelError(string.Empty, "Server error after some time");
                }

                return View(tools);
            }
        }
    }
}