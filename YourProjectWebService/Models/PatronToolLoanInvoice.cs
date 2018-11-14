using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YourProjectWebService.Models
{
    public class PatronToolLoanInvoice
    {
        public long PatronToolLoanId { get; set; }
        public Tool Tool { get; set; }
        public Patron Patron { get; set; }
        public DateTime DateRented { get; set; }
        public DateTime? DateReturned { get; set; }
        public string Workspace { get; set; }
    }
}