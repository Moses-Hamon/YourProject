using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourProjectWebApp.WebServiceYourProject;

namespace YourProjectWebApp.Controllers
{
    public class ToolController : Controller
    {
        // GET: Tool
        public ActionResult Index()
        {
            //create a new client
            var svc = new YourProjectServiceSoapClient();
            // call method from service
            var data = svc.GetAllTools();
            //return data
            return View(data);
        }

        // GET: Tool/Details/5
        public ActionResult Details(int id)
        {
            // open soap client
            var svc = new YourProjectServiceSoapClient();
            // call method and save data
            var data = svc.GetSingleTool(id);
            // check if there was an entry
            if (data != null)
            {
                return View(data);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Item Does not Exist");
                return View("Index");
            }
        }

        // GET: Tool/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tool/Create
        [HttpPost]
        public ActionResult Create(Tool tool)
        {
            // check model
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please enter valid properties");
                return View(tool);
            }
            // create connection to server
            var svc = new YourProjectServiceSoapClient();
            // create the tool
            var results = svc.CreateTool(tool);
            // check the tool
            if (results.Id == 0)
            {
                ModelState.AddModelError(string.Empty, "The item was not Created please try again");
                return View(results);
            }
            else
            {
                TempData["Success"] = "The Item has been added!!!";
                return RedirectToAction("Index");
            }
        }

        // GET: Tool/Edit/5
        public ActionResult Edit(int id)
        {
            //create connection
            var svc = new YourProjectServiceSoapClient();
            // grab the tool
            var tool = svc.GetSingleTool(id);
            //check for null
            if (tool == null)
            {
                ModelState.AddModelError(string.Empty, "Patron does not exist");
                return RedirectToAction("Index");
            }
            return View(tool);
        }

        // POST: Tool/Edit/5
        [HttpPost]
        public ActionResult Edit(Tool tool)
        {
            // check for validation
            if (!ModelState.IsValid)
            {
                return View(tool);
            }
            //open connection
            var svc = new YourProjectServiceSoapClient();
            // Updates the item
            var results = svc.UpdateTool(tool);
            // checks the item
            if (results != null)
            {
                TempData["Success"] = "The Item was Updated!!";
                return RedirectToAction("Index");
            }
            // returns an error
            ModelState.AddModelError(string.Empty, "Item was not updated");
            return View();

        }

        // GET: Tool/Delete/5
        public ActionResult Delete(int id)
        { 
            //open connection
            var svc = new YourProjectServiceSoapClient();
            // grabs item from database
            var tool = svc.GetSingleTool(id);
            // checks item
            if (tool == null)
            {
                ModelState.AddModelError(string.Empty, "tool does not exist");
                return RedirectToAction("Index");
            }

            return View(tool);
        }

        // POST: Tool/Delete/5
        [HttpPost]
        public ActionResult Delete(Tool tool)
        {
            //connects to server
            var svc = new YourProjectServiceSoapClient();
            // Deletes the item
            var results = svc.DeleteTool((int)tool.Id);
            // checks to see if the item was deleted
            if (results)
            {
                TempData["Success"] = "Item was Deleted!!";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Item could not be deleted.");
                return View(tool);
            }
        }
    }
}
