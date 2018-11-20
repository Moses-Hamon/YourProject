using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YourProjectDataService.DAL;
using Dapper;
using System.Data.SQLite;

namespace YourProjectDataService.Model
{
    public class PatronToolLoanInvoice : CRUDMethods, IObjectIdentifier
    {
        [Display(Name = "Invoice #")]
        public long Id { get; set; }

        [Required]
        public int ToolId { get; set; }

        [Required]
        public int PatronId { get; set; }

        [Required]
        public string DateRented { get; set; }

        
        public string DateReturned { get; set; }
        
        public string Workspace { get; set; }

        // Query Strings including parameters
        public const string QuerySelectAll = "SELECT * FROM PatronToolLoan";
        public const string QuerySelectOne = "SELECT * FROM PatronToolLoan WHERE Id = @id";

        public const string QueryInsertInto =
            "INSERT INTO PatronToolLoan (toolId, patronId, dateRented, dateReturned, workspace) VALUES (@ToolId, @PatronId, @dateRented, @dateReturned, @workspace);";

        public const string QueryUpdate =
            "UPDATE PatronToolLoan SET toolId=@toolId, patronId=@patronId, dateRented=@dateRented, dateReturned=@dateReturned, workspace=@workspace WHERE Id=@Id;";

        public const string QueryDelete = "DELETE FROM PatronToolLoan WHERE Id=@id;";


    }
}