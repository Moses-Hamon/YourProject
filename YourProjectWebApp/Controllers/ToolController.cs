using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
            if (!ModelState.IsValid)
            {
                return View(tool);
            }
            //receives request
            //runs create via API
            if (Tool.Create(tool))
            {
                TempData["Success"] = "Tool Created Successfully!!";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error. Please contact Moses");
                return View(tool);
            }
        }

        public ActionResult Edit(int id)
        {
            var tool = Tool.GetSingle(id);

            if (!tool.Equals(null))
            {
                return View(tool);
                }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(Tool tool)
        {
            // check model
            if (!ModelState.IsValid)
            {
                return View(tool);
            }
            // If the tool updates successfully
            if (Tool.Update(tool))
            {
                TempData["Success"] = "Tool Updated Successfully!!";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error Updating tool.");
                return View(tool);
            }
        }

        
        public ActionResult Delete(int id)
        {
            var tool = Tool.GetSingle(id);
            if (tool == null)
            {
                ModelState.AddModelError(string.Empty, $"Item with ID: {id} Does not exist in the database!!!");
                return View("Index");
            }
            //check model

            if (Tool.Delete(tool.ToolId))
            {
                // adds success msg that will display in the index screen
                TempData["Success"] = $"Tool deleted successfully!!";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Error deleting item with id:{id}");
                return RedirectToAction("Index");
            }


        }
    }
}