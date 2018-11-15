using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourProjectWebApp.WebServiceYourProject;

namespace YourProjectWebApp.ViewModels
{
    public class CreateToolWithBrandViewModel
    {
        public Tool Tool { get; set; }
        public IEnumerable<Brand> Brands { get; set; }


    }
}