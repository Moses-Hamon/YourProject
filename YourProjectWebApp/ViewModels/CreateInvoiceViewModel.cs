using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YourProjectWebApp.WebServiceYourProject;

namespace YourProjectWebApp.ViewModels
{
    public class CreateInvoiceViewModel
    {
        public PatronToolLoanInvoice PatronToolLoanInvoice { get; set; }
        public IEnumerable<Patron> Patrons { get; set; }
        public IEnumerable<Tool> Tools { get; set; }
        
    }
}