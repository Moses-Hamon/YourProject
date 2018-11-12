using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YourProjectWebService.Models
{
    public class Tool
    {
        public int ToolId { get; set; }
        public int BrandId { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string Comments { get; set; }
        public bool InUse { get; set; }


    }
}