using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourProjectWebApp.Models;

namespace YourProjectWebApp.ViewModels
{
    public class ToolBrandViewModel
    {

        public Tool Tool { get; set; }
        public IEnumerable<SelectListItem> Brands { get; set; }


        public static IEnumerable<SelectListItem> Convert(IEnumerable<Brand> brands)
        {
            var list = new List<SelectListItem>();
            foreach (var brand in brands)
            {
                list.Add(new SelectListItem()
                {
                    Text = brand.BrandName,
                    Value = brand.BrandId.ToString()
                });
            }

            return list;

        }

    }
}