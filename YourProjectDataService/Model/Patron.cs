using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YourProjectDataService.DAL;

namespace YourProjectDataService.Model
{
    public class Patron : CRUDMethods, IObjectIdentifier
    {
        public long Id { get; set; }
        [Display(Name = "Patron Name")]
        [Required]
        public string PatronName { get; set; }
        
        public bool IsGroup { get; set; }

        public const string QuerySelectAll = "SELECT * FROM Patron";
        public const string QuerySelectOne = "SELECT * FROM Patron WHERE Id = @id";
        public const string QueryInsertInto =
            "INSERT INTO Patron (patronName, isGroup) VALUES (@patronName, @isGroup);";
        public const string QueryUpdate =
            "UPDATE Patron SET patronName=@patronName, isGroup=@isGroup WHERE Id=@Id;";
        public const string QueryDelete = "DELETE FROM Patron WHERE Id=@id;";
    }
}