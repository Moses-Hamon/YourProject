using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YourProjectWebService.Models
{
    public class PatronToolLoanInvoice
    {
        public long PatronToolLoanId { get; set; }
        public long ToolId { get; set; }
        public long PatronId { get; set; }
        public DateTime DateRented { get; set; }
        public DateTime DateReturned { get; set; }
        public string Workspace { get; set; }
    }
}