using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YourProjectWebService.Models
{
    public class PatronToolLoan
    {
        public int PatronToolLoanId { get; set; }
        public Tool ToolId { get; set; }
        public Patron PatronId { get; set; }
        public DateTime DateRented { get; set; }
        public DateTime DateReturned { get; set; }
        public string Workspace { get; set; }
    }
}