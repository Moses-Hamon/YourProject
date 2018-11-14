using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourProjectWebApp.WebServiceYourProject;

namespace YourProjectWebApp.ViewModels
{
    public class ToolBrandViewModel
    {
        public Tool Tool { get; set; }
        public IEnumerable<SelectListItem> Brands { get; set; }

        /// <summary>
        /// Converts the List of brands retrieved from the server and converts into a List of Selectlistitems used for drop down lists
        /// </summary>
        /// <param name="list">List of brands</param>
        /// <returns>List of SelectListItems</returns>
        public static IEnumerable<SelectListItem> ConvertForDropDownList(IEnumerable<Brand> list)
        {
            var newItems = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "Please Select One" , Selected = true, Value = null}
            };
            // interates through the list and added them into a Select List list
            foreach (var variable in list)
            {
                newItems.Add(new SelectListItem(){ Text = variable.BrandName, Value = variable.Id.ToString()});
            }

            return newItems;
        }
    }
}