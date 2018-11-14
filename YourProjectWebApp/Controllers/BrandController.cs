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

        // GET: Brand/Details/5
        public ActionResult Details(int id)
        {
            // open soap client
            var svc = new YourProjectServiceSoapClient();
            // call method and save data
            var data = svc.GetSingleBrand(id);
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

        // GET: Brand/Create
        public ActionResult Create()
        {
            return View();
        }

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
