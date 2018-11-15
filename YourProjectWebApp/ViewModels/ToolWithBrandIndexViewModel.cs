using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YourProjectWebApp.WebServiceYourProject;

namespace YourProjectWebApp.ViewModels
{
    public class ToolWithBrandIndexViewModel
    {
        public IEnumerable<Tool> Tools { get; set; }
        public IEnumerable<Brand> Brands { get; set; }

    }
}