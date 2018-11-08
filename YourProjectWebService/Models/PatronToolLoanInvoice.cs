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

        [Required]
        public Tool Tool { get; set; }

        [Required]
        public Patron Patron { get; set; }

        [Required]
        public DateTime DateRented { get; set; }

        // ? allows the property to be nullable as we will not know the actual date returned yet.
        public DateTime? DateReturned { get; set; }
        public string Workspace { get; set; }
    }
}