using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourProjectWebApp.ViewModels;
using YourProjectWebApp.WebServiceYourProject;

namespace YourProjectWebApp.Controllers
{
    public class ToolController : Controller
    {
        /// <summary>
        /// Index for setting up the list of Tools
        /// </summary>
        /// <returns></returns>
        // GET: Tool
        public ActionResult Index()
        {
            //create a new client
            var svc = new YourProjectServiceSoapClient();
            // populate 
            var viewModel = new ToolWithBrandIndexViewModel
            {
                Tools = svc.GetAllTools(),
                Brands = svc.GetAllBrands()
        };

            //return data
            return View(viewModel);
        }
        /// <summary>
        /// Sets up the details page for a single tool
        /// </summary>
        /// <param name="id">Id of the selected tool</param>
        /// <returns></returns>
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
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// sets up create view for creating a new tool
        /// </summary>
        /// <returns></returns>
        // GET: Tool/Create
        public ActionResult Create()
        {
            var svc = new YourProjectServiceSoapClient();
            var viewModel = new CreateToolWithBrandViewModel()
            {
                Tool = new Tool { Active = true},
                Brands = svc.GetAllBrands()
            };
            return View(viewModel);
        }
        /// <summary>
        /// Creates a new tool via the webservice
        /// </summary>
        /// <param name="tool">Model containing the form data</param>
        /// <returns></returns>
        // POST: Tool/Create
        [HttpPost]
        public ActionResult Create(Tool tool)
        {
            // create connection to server
            var svc = new YourProjectServiceSoapClient();
            // creates a model for the view
            var viewModel = new CreateToolWithBrandViewModel()
            {
                
                Brands = svc.GetAllBrands()
            }; 
            // check model
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please enter valid properties");
                return View(viewModel);
            }
            // add the brand name onto the tool
            var brandForTool = svc.GetSingleBrand(tool.BrandId);
            tool.Description = string.Join(" ", brandForTool.BrandName, tool.Description);
            // create the tool
            var results = svc.CreateTool(tool);
            viewModel.Tool = results;
            // check the tool
            if (results.Id == 0)
            {
                
                ModelState.AddModelError(string.Empty, "The item was not Created please try again");
                return View(viewModel);
            }
            else
            {
                TempData["Success"] = "The Item has been added!!!";
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// Sets up the edit view for a single tool
        /// </summary>
        /// <param name="id">Id of the Tool</param>
        /// <returns></returns>
        // GET: Tool/Edit/5
        public ActionResult Edit(int id)
        {
            //create connection
            var svc = new YourProjectServiceSoapClient();
            // grab the tool
            var tool = svc.GetSingleTool(id);
            var brands = svc.GetAllBrands();
            //check for null
            if (tool == null || brands == null)
            {
                ModelState.AddModelError(string.Empty, "Item does not exist");
                return RedirectToAction("Index");
            }

            var viewModel = new CreateToolWithBrandViewModel()
            {
                Tool = tool,
                Brands = brands
            };
            return View(viewModel);
        }
        /// <summary>
        /// Updates the tool from the form info submitted
        /// </summary>
        /// <param name="tool">Model containing form data</param>
        /// <returns></returns>
        // POST: Tool/Edit/5
        [HttpPost]
        public ActionResult Edit(Tool tool)
        {
            //open connection
            var svc = new YourProjectServiceSoapClient();
            var viewModel = new CreateToolWithBrandViewModel()
            {
                Brands = svc.GetAllBrands(),
                Tool = tool
            };
            // check for validation
            if (!ModelState.IsValid)
            {
               
                ModelState.AddModelError(string.Empty, "Incorrect Model");
                return View(viewModel);
            }
            
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
        /// <summary>
        /// Deletes the selected item using the webservice using the Id
        /// </summary>
        /// <param name="id">Id of the Tool for deletion</param>
        /// <returns></returns>
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
        /// <summary>
        /// Changes the active attribute of the tool (retire)
        /// </summary>
        /// <param name="id">Id of the tool</param>
        /// <returns></returns>
        public ActionResult RetireTool(int id)
        {
            var svc = new YourProjectServiceSoapClient();
            var tool = svc.GetSingleTool(id);
            tool.Active = false;
            var updatedTool = svc.UpdateTool(tool);
            if (tool.Active != updatedTool.Active)
            {
                TempData["Success"] = "Tool has been retired";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty,"There was a problem updating the active tool");
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Delete the tool from the database using the webservice
        /// </summary>
        /// <param name="tool">Model containing the Item for deletion</param>
        /// <returns></returns>
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
