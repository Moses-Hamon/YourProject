using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YourProjectWebApp.WebServiceYourProject;

namespace YourProjectWebApp.ViewModels
{
    public class EditInvoiceViewModel
    {
        public PatronToolLoanInvoice PatronToolLoanInvoice { get; set; }
        public int OldTool { get; set; }
        public IEnumerable<Patron> Patrons { get; set; }
        public IEnumerable<Tool> Tools { get; set; }

    
}
}