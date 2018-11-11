using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YourProjectWebApp.Models;

namespace YourProjectWebApp.Controllers
{
    public class ToolController : Controller
    {
        // GET: Tool
        public async Task<ActionResult> Index()
        {
            var tools = await Tool.GetAll();
            return View(tools);
        }

        // GET: Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        public ActionResult Create(Tool tool)
        {
            //receives request
            //runs create via API
            if (Tool.Create(tool))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error. Please contact Moses");
                return View(tool);
            }
        }

        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult Edit(long id)
        {
            throw new NotImplementedException();
        }
    }
}