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
        // GET: ALL Tools
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult RetrieveReport(string queryType, int? brand)
        {

            // open connection
            var svc = new YourProjectServiceSoapClient();
            var viewModel = new ToolWithBrandIndexViewModel
            {
                Brands = svc.GetAllBrands()
            };

            // store list based on query and brand
            viewModel.Tools = svc.GetAllToolsWithCondition(brand == null 
                ? SelectQuery(queryType, null)
                : SelectQuery(queryType, brand));

            // Store the queryName
            TempData.Add("query", queryType);
            TempData.Add("brand", brand);
            // return partial view with new info
            return PartialView(viewModel);
        }

        public FileResult DownloadCSV(string queryType, int? brand)
        {
            var builder = new StringBuilder();
            var svc = new YourProjectServiceSoapClient();
            var model = svc.GetAllToolsWithCondition(brand == null
                ? SelectQuery(queryType, null)
                : SelectQuery(queryType, brand));

            foreach (var tool in model)
            {
                builder.AppendLine(ConvertToolToCsvString(tool));
            }
            
            return File(new UTF8Encoding().GetBytes(builder.ToString()), "text/csv", $"{queryType}_{DateTime.Now.ToShortDateString()}_Report.csv");
        }

        public string ConvertToolToCsvString(Tool tool)
        {
            var svc = new YourProjectServiceSoapClient();
            var brand = svc.GetSingleBrand(tool.BrandId);
            // using the tools properties and inbuilt function that .net provides Creates a string out of our model
            return string.Join(",", new object[] { tool.Id, tool.Description, brand.BrandName, tool.Comments, tool.Active, tool.InUse });
        }

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