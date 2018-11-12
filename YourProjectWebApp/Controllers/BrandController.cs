using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YourProjectWebApp.Models;

namespace YourProjectWebApp.Controllers
{
    public class BrandController : Controller
    {
        // GET: Brand
        public async Task<ActionResult> Index()
        {
            var brand = await Brand.GetAll();
            return View(brand);
        }

        // GET: Brand/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Brand/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brand/Create
        [HttpPost]
        public ActionResult Create(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View(brand);
            }

            if (Brand.Create(brand))
            {
                TempData["Success"] = "Brand Created Successfully!!";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error. Please contact Moses");
                return View(brand);
            }
        }

        // GET: Brand/Edit/5
        public ActionResult Edit(int id)
        {
            // retrieve item using api
            var brand = Brand.GetSingle(id);

            // checks to see if the item exists
            if (!brand.Equals(null))
            {
                // returns item to the view ready for editing
                return View(brand);
            }
            else
            {
                // adds error to pass onto view
                ModelState.AddModelError(string.Empty, "The item does not exist");
                return RedirectToAction("Index");
            }
        }

        // POST: Brand/Edit/5
        [HttpPost]
        public ActionResult Edit(Brand brand)
        {
            // check model
            if (!ModelState.IsValid)
            {
                return View(brand);
            }
            // If the item updates successfully
            if (Brand.Update(brand))
            {
                // adds success message
                TempData["Success"] = "Tool Updated Successfully!!";
                return RedirectToAction("Index");
            }
            else
            {
                //provides error message and returns to the edit screen
                ModelState.AddModelError(string.Empty, "Error Updating tool.");
                return View(brand);
            }
        }

        // POST: Brand/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // grabs the item from the database
            var brand = Brand.GetSingle(id);
            // checks if the item exists
            if (brand == null)
            {
                // creates error message if the item does not exist
                ModelState.AddModelError(string.Empty, $"Item with ID: {id} Does not exist in the database!!!");
                return View("Index");
            }
            // executes the delete method which calls api for deletion
            if (Brand.Delete(brand.BrandId))
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
