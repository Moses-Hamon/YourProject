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
            var brands = svc.GetAllBrands();
            
            return View(brands);
        }
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
                new object[] {"Id", "Description", "Brand", "Comments", "Active", "InUse"}));
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
    }
}