using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YourProjectWebApp.ViewModels;
using YourProjectWebApp.WebServiceYourProject;

namespace YourProjectWebApp.Controllers
{
    public class ReportsController : Controller
    {
        /// <summary>
        /// Sets up main page for reports
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var svc = new YourProjectServiceSoapClient();
            var viewModel = new ReportsIndexViewModel
            {
                Brands = svc.GetAllBrands(),
                Patrons = svc.GetAllPatrons(),
                Tools = svc.GetAllTools()
            };
            
            return View(viewModel);
        }

        #region InvoiceReports

        /// <summary>
        /// Creates a Csv file of the report for downloading
        /// </summary>
        /// <param name="model">Data from Report</param>
        /// <returns>Data presented in CSV format</returns>
        public FileContentResult DownloadInvoice(InvoiceIndexViewModel model)
        {
            var document = ConvertToCSV(model);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = $"{DateTime.Now.ToShortDateString()}_Report.csv",
                Inline = false
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(new UTF8Encoding().GetBytes(document), "text/csv");
        }

        /// <summary>
        /// Retrieves the invoice for selected filter (brand or patron)
        /// </summary>
        /// <param name="id">Id if the filter</param>
        /// <param name="conditionType">Type of filter</param>
        /// <returns></returns>
        public ActionResult RetrieveInvoice(int id, string conditionType)
        {
            // open connection
            var svc = new YourProjectServiceSoapClient();
            // create new view model
            var viewModel = new InvoiceIndexViewModel
            {
                Patrons = svc.GetAllPatrons(),
                Tools = svc.GetAllTools()
            };
            // populate viewModel based on conditions
            if (conditionType == "patron")
            {
                var patron = svc.GetSinglePatron(id);
                viewModel.PatronToolLoanInvoices = svc.GetAllInvoicesWithCondition($" WHERE patronId={id}");
                ViewBag.filteredBy = patron.PatronName;
            }
            else
            {
                var tool = svc.GetSingleTool(id);
                viewModel.PatronToolLoanInvoices = svc.GetAllInvoicesWithCondition($" WHERE toolId={id}");
                ViewBag.filteredBy = tool.Description;
            }

            return PartialView(viewModel);
        }

        /// <summary>
        /// Converts the model into CSV format
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        public string ConvertToCSV(InvoiceIndexViewModel model)
        {
            // string builder for creating csv file
            var builder = new StringBuilder();
            // add in the headings
            builder.AppendLine(string.Join(",",
                new object[] { "Id", "ToolId", "PatronId", "DateRented", "DateReturned", "Workspace" }));
            // creates connection
            var svc = new YourProjectServiceSoapClient();
            // for each tool in the list
            foreach (var invoice in model.PatronToolLoanInvoices)
            {
                // add a new line of data in csv format
                builder.AppendLine(ConvertInvoiceToCsvString(invoice));
            }

            // returns file for download.
            //return File(new UTF8Encoding().GetBytes(builder.ToString()), "text/csv", $"{queryType}_{DateTime.Now.ToShortDateString()}_Report.csv");
            return builder.ToString();

        }

        /// <summary>
        /// Converts a single invoice into CSV string
        /// </summary>
        /// <param name="invoice">invoice model</param>
        /// <returns></returns>
        public string ConvertInvoiceToCsvString(PatronToolLoanInvoice invoice)
        {
            var svc = new YourProjectServiceSoapClient();
            // collect the corresponding tools and patrons
            var tool = svc.GetSingleTool(invoice.ToolId);
            var patron = svc.GetSinglePatron(invoice.PatronId);
            // using the tools properties and inbuilt function that .net provides Creates a string out of our model
            return string.Join(",", new object[] {invoice.Id, tool.Description, patron.PatronName, invoice.DateRented, invoice.DateReturned, invoice.Workspace });
        }

        #endregion


        #region ToolReports

        /// <summary>
        /// Retrieves a report based on the query and a brand
        /// Places view into an existing div
        /// </summary>
        /// <param name="queryType">The query title for the report</param>
        /// <param name="brand">id of the brand</param>
        /// <returns></returns>
        public ActionResult RetrieveReport(string queryType, int? brand)
        {

            // open connection
            var svc = new YourProjectServiceSoapClient();
            var viewModel = new ToolWithBrandIndexViewModel
            {
                // Brands are used for displaying brand name;
                Brands = svc.GetAllBrands(),
                // store list based on query and brand
                Tools = svc.GetAllToolsWithCondition(brand == null
                    ? SelectQuery(queryType, null)
                    : SelectQuery(queryType, brand))
            };
            // Store the queryName
            ViewBag.query = queryType;
            ViewBag.brand = brand;
            // return partial view with new info
            return PartialView(viewModel);
        }

        /// <summary>
        /// Returns the converted file for downloading.
        /// 
        /// </summary>
        /// <param name="queryType">Query title</param>
        /// <param name="brand">brand id for filtering</param>
        /// <returns></returns>
        public FileContentResult Download(string queryType, int? brand)
        {
            var document = ConvertToCSV(queryType, brand);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = $"{DateTime.Now.ToShortDateString()}_Report.csv",
                Inline = false
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(new UTF8Encoding().GetBytes(document), "text/csv");
        }

        /// <summary>
        /// Retrieves the data using webservice
        /// Converts the data into CSV format
        /// </summary>
        /// <param name="queryType">Query title</param>
        /// <param name="brand">Brand Id for filtering</param>
        /// <returns></returns>
        public string ConvertToCSV(string queryType, int? brand)
        {
            // string builder for creating csv file
            var builder = new StringBuilder();
            // add in the headings
            builder.AppendLine(string.Join(",",
                new object[] { "Id", "Description", "Brand", "Comments", "Active", "InUse" }));
            // creates connection
            var svc = new YourProjectServiceSoapClient();
            // collects info
            var model = svc.GetAllToolsWithCondition(brand == null
                ? SelectQuery(queryType, null)
                : SelectQuery(queryType, brand));
            // for each tool in the list
            foreach (var tool in model)
            {
                // add a new line of data in csv format
                builder.AppendLine(ConvertToolToCsvString(tool));
            }

            // returns file for download.
            //return File(new UTF8Encoding().GetBytes(builder.ToString()), "text/csv", $"{queryType}_{DateTime.Now.ToShortDateString()}_Report.csv");
            return builder.ToString();

        }

        /// <summary>
        /// Converts a single tool to csv format
        /// </summary>
        /// <param name="tool">Tool that is converted to csv string</param>
        /// <returns></returns>
        public string ConvertToolToCsvString(Tool tool)
        {
            var svc = new YourProjectServiceSoapClient();
            var brand = svc.GetSingleBrand(tool.BrandId);
            // using the tools properties and inbuilt function that .net provides Creates a string out of our model
            return string.Join(",", new object[] { tool.Id, tool.Description, brand.BrandName, tool.Comments, tool.Active, tool.InUse });
        }

        /// <summary>
        /// Selector used for selecting queryType and brand Id
        /// </summary>
        /// <param name="queryType">QueryType</param>
        /// <param name="brandId">Brand id </param>
        /// <returns>Completed Query</returns>
        public string SelectQuery(string queryType, int? brandId)
        {
            var queryString = "";
            switch (queryType)
            {
                // Filters
                // For all checked out tools.
                case "AllChecked":
                    queryString = " WHERE inUse='1';";
                    break;
                // All active tools.
                case "AllActive":
                    queryString = " WHERE active='1'";
                    break;
                // All active tools for a specific brand
                case "AllActiveByBrand":
                    queryString = $" WHERE active='1' AND brandId='{brandId}';";
                    break;
                // All retired tools
                case "AllRetired":
                    queryString = " WHERE active='0';";
                    break;
                // All retired tools for a specific brand.
                case "AllRetiredByBrand":
                    queryString = $" WHERE active='0' AND brandId='{brandId}';";
                    break;
                // do not modify the query (defaults to SELECT ALL)
                default:
                    queryString = "";
                    break;
            }

            return queryString;
        }

        #endregion

    }
}