using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YourProjectWebApp.Models;

namespace YourProjectWebApp.ViewModels
{
    public class ToolViewModel
    {
        public long Id { get; set; }
        public Tool Tool { get; set; }
        public Brand Brand { get; set; }

    }
}