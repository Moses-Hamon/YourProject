using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YourProjectDataService.DAL;

namespace YourProjectDataService.Model
{
    public class Brand : CRUDMethods, IObjectIdentifier
    {
        public long Id { get; set; }
        [Required]
        public string BrandName { get; set; }

        public const string QuerySelectAll = "SELECT * FROM Brand";
        public const string QuerySelectOne = "SELECT * FROM Brand WHERE Id = @id";

        public const string QueryInsertInto =
            "INSERT INTO Brand (brandName) VALUES (@brandName);";

        public const string QueryUpdate =
            "UPDATE Brand SET brandName=@brandName WHERE Id=@Id;";

        public const string QueryDelete = "DELETE FROM Brand WHERE Id=@id;";
    }
}