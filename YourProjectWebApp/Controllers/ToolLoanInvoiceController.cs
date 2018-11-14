﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourProjectWebApp.ViewModels;
using YourProjectWebApp.WebServiceYourProject;

namespace YourProjectWebApp.Controllers
{
    public class ToolLoanInvoiceController : Controller
    {
        // GET: ToolLoanInvoice
        public ActionResult Index()
        {
            var svc = new YourProjectServiceSoapClient();
            var viewModel = new InvoiceIndexViewModel
            {
                PatronToolLoanInvoices = svc.GetAllInvoices(),
                Tools = svc.GetAllTools(),
                Patrons = svc.GetAllPatrons()
            };
            

            return View(viewModel);
        }

        // GET: ToolLoanInvoice/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ToolLoanInvoice/Create
        public ActionResult Create()
        {
            var svc = new YourProjectServiceSoapClient();
            var viewModel = new CreateInvoiceViewModel
            {
                Tools = svc.GetAllTools(),
                Patrons = svc.GetAllPatrons()
            };
            return View(viewModel);
        }

        // POST: ToolLoanInvoice/Create
        [HttpPost]
        public ActionResult Create(CreateInvoiceViewModel model)
        {
            var svc = new YourProjectServiceSoapClient();
            
            var result = svc.CreatePatronToolLoanInvoice(model.PatronToolLoanInvoice);

            //var viewModel = new CreateInvoiceViewModel
            //{
            //    PatronToolLoanInvoice = result,
            //    Tools = svc.GetAllTools(),
            //    Patrons = svc.GetAllPatrons()
            //};

            model.Tools = svc.GetAllTools();
            model.Patrons = svc.GetAllPatrons();
            
            if (result == null)
            {
                ModelState.AddModelError(string.Empty,"Error Creating Invoice");
                return View(model);
            }
            else
            {
                TempData["Success"] = "Invoice was created!!!";
                return RedirectToAction("Index");
            }
            
            

            return View();
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
