using System;

namespace YourProjectWebApp.Models
{
    public class PatronToolLoan
    {
        public int PatronToolLoanId { get; set; }
        public long ToolId { get; set; }
        public long PatronId { get; set; }
        public DateTime DateRented { get; set; }
        public DateTime DateReturned { get; set; }
        public string Workspace { get; set; }
    }
}