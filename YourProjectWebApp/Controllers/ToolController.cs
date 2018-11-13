using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YourProjectWebApp.Models;
using YourProjectWebApp.ViewModels;

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
        public async Task<ActionResult> Create()
        { 
            // Grabs all available Brands
            var brands = await Brand.GetAll();


            var viewModel = new ToolBrandViewModel()
            {
                Brands = ToolBrandViewModel.Convert(brands),
                
            };

            return View(viewModel);
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
            // retrieve item using api
            var tool = Tool.GetSingle(id);
            // checks to see if the item exists
            if (!tool.Equals(null))
            {   
                // returns item to the view ready for editing
                return View(tool);
            }
            else
            {
                // adds error to pass onto view
                ModelState.AddModelError(string.Empty, "The item does not exist");
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
            // If the item updates successfully
            if (Tool.Update(tool))
            {
                // adds success message
                TempData["Success"] = "Tool Updated Successfully!!";
                return RedirectToAction("Index");
            }
            else
            {
                //provides error message and returns to the edit screen
                ModelState.AddModelError(string.Empty, "Error Updating tool.");
                return View(tool);
            }
        }
        
        public ActionResult Delete(int id)
        {
            // grabs the item from the database
            var tool = Tool.GetSingle(id);
            // checks if the item exists
            if (tool == null)
            {
                // creates error message if the item does not exist
                ModelState.AddModelError(string.Empty, $"Item with ID: {id} Does not exist in the database!!!");
                return View("Index");
            }
            // executes the delete method which calls api for deletion
            if (Tool.Delete(tool.ToolId))
            {
                // adds success msg that will display in the index screen
                TempData["Success"] = $"Tool deleted successfully!!";
                return RedirectToAction("Index");
            }
            else
            {
                // adds error message and returns to index
                ModelState.AddModelError(string.Empty, $"Error deleting item with id:{id}");
                return RedirectToAction("Index");
            }


        }
    }
}