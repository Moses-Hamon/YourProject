using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using YourProjectWebApp.WebServiceYourProject;

namespace YourProjectWebApp.Controllers
{
    public class PatronController : Controller
    {
        /// <summary>
        /// Sets up index view for patron
        /// </summary>
        /// <returns></returns>
        // GET: Patron
        public ActionResult Index()
        {
            //create a new client
            var svc = new YourProjectServiceSoapClient();
            // call method from service
            var data = svc.GetAllPatrons();
            //return data
            return View(data);
        }
        /// <summary>
        ///  Sets up View for details of a single Patron
        /// </summary>
        /// <param name="id">Id of the Patron</param>
        /// <returns></returns>
        // GET: Patron/Details/5
        public ActionResult Details(int id)
        {
            // open soap client
            var svc = new YourProjectServiceSoapClient();
            // call method and save data
            var data = svc.GetSinglePatron(id);
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
        /// <summary>
        /// Sets u view for creating a new Patron
        /// </summary>
        /// <returns></returns>
        // GET: Patron/Create
        public ActionResult Create()
        {
            
            return View();
        }
        /// <summary>
        /// Creates a new patron using model data from form collection
        /// creates new patron in database using web services
        /// </summary>
        /// <param name="patron">Model with form data</param>
        /// <returns></returns>
        // POST: Patron/Create
        [HttpPost]
        public ActionResult Create(Patron patron)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please enter valid properties");
                return View(patron);
            }
            // create connection to server
            var svc = new YourProjectServiceSoapClient();
            // create the tool
            var results = svc.CreatePatron(patron);
            // check the tool
            if (results.Id == 0)
            {
                ModelState.AddModelError(string.Empty , "The item was not Created please try again");
                return View(results);
            }
            else
            {
                TempData["Success"] = "The Item was created";
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// sets up screen for editing a single patron
        /// </summary>
        /// <param name="id">Id of patron</param>
        /// <returns></returns>
        // GET: Patron/Edit/5
        public ActionResult Edit(int id)
        {
            //create connection
            var svc = new YourProjectServiceSoapClient();
            // grab the tool
            var patron = svc.GetSinglePatron(id);
            //check for null
            if (patron == null)
            {
                ModelState.AddModelError(string.Empty, "Patron does not exist");
                return RedirectToAction("Index");
            }
            return View(patron);
        }
        /// <summary>
        /// Updates a patron using the Model data collected from form
        /// </summary>
        /// <param name="patron">Model with form data</param>
        /// <returns></returns>
        // POST: Patron/Edit/5
        [HttpPost]
        public ActionResult Edit(Patron patron)
        {
            // check for validation
            if (!ModelState.IsValid)
            {
                return View(patron);
            }
            //open connection
            var svc = new YourProjectServiceSoapClient();
            // Updates the item
            var results = svc.UpdatePatron(patron);
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
        /// sets up a delete screen for a single patron
        /// </summary>
        /// <param name="id">Id of Patron</param>
        /// <returns></returns>
        // GET: Patron/Delete/5
        public ActionResult Delete(int id)
        {
            //open connection
            var svc = new YourProjectServiceSoapClient();
            // grabs item from database
            var patron = svc.GetSinglePatron(id);
            // checks item
            if (patron == null)
            {
                ModelState.AddModelError(string.Empty, "Patron does not exist");
                return RedirectToAction("Index");
            }

            return View(patron);
        }
        /// <summary>
        /// Deletes a patron entry from the database using web service
        /// </summary>
        /// <param name="patron">Patron model</param>
        /// <returns></returns>
        // POST: Patron/Delete/5
        [HttpPost]
        public ActionResult Delete(Patron patron)
        {
            //connects to server
            var svc = new YourProjectServiceSoapClient();
            // Deletes the item
            var results = svc.DeletePatron((int) patron.Id);
            // checks to see if the item was deleted
            if (results)
            {
                TempData["Success"] = "Item was Deleted!!";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Item could not be deleted.");
                return View(patron);
            }
        }
    }
}
