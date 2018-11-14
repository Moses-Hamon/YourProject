using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourProjectWebApp.WebServiceYourProject;

namespace YourProjectWebApp.Controllers
{
    public class ToolLoanInvoiceController : Controller
    {
        // GET: ToolLoanInvoice
        public ActionResult Index()
        {
            var svc = new YourProjectServiceSoapClient();
            var model = svc.GetAllInvoices();

            return View(model);
        }

        // GET: ToolLoanInvoice/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ToolLoanInvoice/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToolLoanInvoice/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ToolLoanInvoice/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ToolLoanInvoice/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ToolLoanInvoice/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ToolLoanInvoice/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
