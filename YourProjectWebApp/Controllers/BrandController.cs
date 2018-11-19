using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourProjectWebApp.WebServiceYourProject;

namespace YourProjectWebApp.Controllers
{
    public class BrandController : Controller
    {
        /// <summary>
        /// Sets up the index for brands
        /// </summary>
        /// <returns></returns>
        // GET: Brand
        public ActionResult Index()
        {
            //create a new client
            var svc = new YourProjectServiceSoapClient();
            // call method from service
            var data = svc.GetAllBrands();
            //return data
            return View(data);
        }
        /// <summary>
        /// Sets up the create a new brand view
        /// </summary>
        /// <returns></returns>
        // GET: Brand/Create
        public ActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Creates a new brand using form data
        /// </summary>
        /// <param name="brand">Id of brand</param>
        /// <returns></returns>
        // POST: Brand/Create
        [HttpPost]
        public ActionResult Create(Brand brand)
        {
            // check model
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please enter valid properties");
                return View(brand);
            }
            // create connection to server
            var svc = new YourProjectServiceSoapClient();
            // create the brand
            var results = svc.CreateBrand(brand);
            // check the brand
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
        /// <summary>
        /// Sets up the view for editing a single brand
        /// </summary>
        /// <param name="id">Id of the brand</param>
        /// <returns></returns>
        // GET: Brand/Edit/5
        public ActionResult Edit(int id)
        {
            //create connection
            var svc = new YourProjectServiceSoapClient();
            // grab the brand
            var brand = svc.GetSingleBrand(id);
            //check for null
            if (brand == null)
            {
                ModelState.AddModelError(string.Empty, "Patron does not exist");
                return RedirectToAction("Index");
            }
            return View(brand);
        }
        /// <summary>
        /// Updates the brand using form data
        /// Updates brand in database using web service
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        // POST: Brand/Edit/5
        [HttpPost]
        public ActionResult Edit(Brand brand)
        {
            // check for validation
            if (!ModelState.IsValid)
            {
                return View(brand);
            }
            //open connection
            var svc = new YourProjectServiceSoapClient();
            // Updates the item
            var results = svc.UpdateBrand(brand);
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
        /// Sets up the view for deleting a single brand
        /// </summary>
        /// <param name="id">Id of brand</param>
        /// <returns></returns>
        // GET: Brand/Delete/5
        public ActionResult Delete(int id)
        {
            //open connection
            var svc = new YourProjectServiceSoapClient();
            // grabs item from database
            var brand = svc.GetSingleBrand(id);
            // checks item
            if (brand == null)
            {
                ModelState.AddModelError(string.Empty, "brand does not exist");
                return RedirectToAction("Index");
            }

            return View(brand);
        }
        /// <summary>
        /// Deletes the brand based on the form data using the web service
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        // POST: Brand/Delete/5
        [HttpPost]
        public ActionResult Delete(Brand brand)
        {
            //connects to server
            var svc = new YourProjectServiceSoapClient();
            // Deletes the item
            var results = svc.DeleteBrand((int)brand.Id);
            // checks to see if the item was deleted
            if (results)
            {
                TempData["Success"] = "Item was Deleted!!";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Item could not be deleted.");
                return View(brand);
            }
        }
    }
}
