using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourProjectWebApp.ViewModels;
using YourProjectWebApp.WebServiceYourProject;

namespace YourProjectWebApp.Controllers
{
    public class ToolLoanInvoiceController : Controller
    {
        /// <summary>
        /// Index for Loan Invoices, Collects all invoices
        /// also contains lists of tools and patrons for drop down lists
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Sets up the view for Creating a new Tool loan invoice
        /// </summary>
        /// <returns></returns>
        // GET: ToolLoanInvoice/Create
        public ActionResult Create()
        {
            var svc = new YourProjectServiceSoapClient();
            var viewModel = new CreateInvoiceViewModel
            {
                
                Tools = svc.GetAllToolsWithCondition(" WHERE inUse='0' AND active='1';"),
                Patrons = svc.GetAllPatrons()
            };
            return View(viewModel);
        }
        /// <summary>
        /// Creates an entry for Tool Loan Invoice using the form data from the create page
        /// </summary>
        /// <param name="model">Model that holds the info</param>
        /// <returns></returns>
        // POST: ToolLoanInvoice/Create
        [HttpPost]
        public ActionResult Create(CreateInvoiceViewModel model)
        {
            var svc = new YourProjectServiceSoapClient();
            // create entry in the database
            var result = svc.CreatePatronToolLoanInvoice(model.PatronToolLoanInvoice);
            // add list of tools and patrons for drop down lists
            model.Tools = svc.GetAllTools();
            model.Patrons = svc.GetAllPatrons();
            // check the result
            if (result == null)
            {
                ModelState.AddModelError(string.Empty,"Error Creating Invoice");
                return View(model);
            }
            else
            {
                // update the tool used
                var toolToBeUpdated = svc.GetSingleTool(result.ToolId);
                toolToBeUpdated.InUse = true;
                svc.UpdateTool(toolToBeUpdated);

                TempData["Success"] = "Invoice was created!!!";
                return RedirectToAction("Index");
            }

        }
        /// <summary>
        /// Sets up the edit view for editing a ToolLoanInvoice
        /// </summary>
        /// <param name="id">Id of the invoice</param>
        /// <returns></returns>
        // GET: ToolLoanInvoice/Edit/5
        public ActionResult Edit(int id)
        {
            // open connection
            var svc = new YourProjectServiceSoapClient();

            // populate model for the view
            var viewModel = new EditInvoiceViewModel
            {
                PatronToolLoanInvoice = svc.GetSinglePatronToolLoanInvoice(id),
                //only grab available tools
                Tools = svc.GetAllToolsWithCondition(" WHERE inUse='0';"),
                Patrons = svc.GetAllPatrons()
            };

            return View(viewModel);
        }
        /// <summary>
        /// Updates the entry via the webservice using the model from the form.
        /// </summary>
        /// <param name="model">Model that contains the data</param>
        /// <returns></returns>
        // POST: ToolLoanInvoice/Edit/5
        [HttpPost]
        public ActionResult Edit(EditInvoiceViewModel model)
        {
            // open the model
             var svc = new YourProjectServiceSoapClient();
            // update the model
            var updatedModel = svc.UpdatePatronToolLoanInvoice(model.PatronToolLoanInvoice);
            // check model
            if (updatedModel == null)
            {
                return View(model);
            }
            // return
            TempData["Success"] = "Updated Successfully";
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Deletes the entry using the ID via the web service.
        /// </summary>
        /// <param name="id">Id of the entry for deletion</param>
        /// <returns></returns>
        // GET: ToolLoanInvoice/Delete/5
        public ActionResult Delete(int id)
        {
            //connect
            var svc = new YourProjectServiceSoapClient();
            var invoiceForDeletion = svc.GetSinglePatronToolLoanInvoice(id);
            var results = svc.DeletePatronToolLoanInvoice(id);
            // if it has been deleted
            if (results)
            {
                // grab tool
                var toolToBeUpdated = svc.GetSingleTool(invoiceForDeletion.ToolId);
                // update tool
                toolToBeUpdated.InUse = false;
                //update in db
                var updatedTool = svc.UpdateTool(toolToBeUpdated);

                TempData["Success"] = "Deletion Successful";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Item was not Updated");
            return View("Index");
        }

       

        /// <summary>
        /// Partial View for Invoice Details
        /// </summary>
        /// <param name="id">Id of the invoice for retrieval</param>
        /// <returns>View</returns>
        public ActionResult _invoiceDetails(int id)
        {
            var svc = new YourProjectServiceSoapClient();
            var invoice = svc.GetSinglePatronToolLoanInvoice(id);

            var viewModel = new InvoiceDetailsViewModel
            {
                ToolLoanInvoice = invoice,
                Patron = svc.GetSinglePatron(invoice.PatronId),
                Tool = svc.GetSingleTool(invoice.ToolId)
            };

            return PartialView(viewModel);
        }

        /// <summary>
        /// Updates a PatronLoanInvoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Message </returns>
        public ActionResult UpdateReturn(int id)
        {
            //connect
            var svc = new YourProjectServiceSoapClient();
            // grab the invoice containing the tool (to be updated)
            var invoiceContainingTool = svc.GetSinglePatronToolLoanInvoice(id);
            // Update the info
            invoiceContainingTool.DateReturned = DateTime.Now.ToShortDateString();
            // Update the database
            var updatedInvoice = svc.UpdatePatronToolLoanInvoice(invoiceContainingTool);
            // grab the tool (to be updated)
            var toolForUpdating = svc.GetSingleTool(invoiceContainingTool.ToolId);
            // update the tool
            toolForUpdating.InUse = false;
            // update the database
            var updatedTool = svc.UpdateTool(toolForUpdating);

            if (updatedTool != null && updatedInvoice != null)
            {
                TempData["Success"] = "Tool and Invoice Successfully updated";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Return was unsuccessful please try again");
                return RedirectToAction("Index");
            }


        }
    }
}
