using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YourProjectWebService.Models
{
    public class Patron
    {
        public int PatronId { get; set; }
        public string PatronName { get; set; }
        public bool IsGroup { get; set; }
    }
}