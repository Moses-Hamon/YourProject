using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YourProjectWebApp.WebServiceYourProject;

namespace YourProjectWebApp.ViewModels
{
    public class InvoiceDetailsViewModel
    {
        public PatronToolLoanInvoice ToolLoanInvoice { get; set; }
        public Patron Patron { get; set; }
        public Tool Tool { get; set; }
        

    }
}