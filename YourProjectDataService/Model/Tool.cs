using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using Dapper;
using YourProjectDataService.DAL;

namespace YourProjectDataService.Model
{
    public class Tool : CRUDMethods, IObjectIdentifier
    {
        public long Id { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        public string Description { get; set; }
        
        public bool Active { get; set; }
        public string Comments { get; set; }
        public bool InUse { get; set; }



        // Query Strings including parameters
        public const string QuerySelectAll = "SELECT * FROM Tool";
        public const string QuerySelectOne = "SELECT * FROM Tool WHERE Id = @id";

        public const string QueryInsertInto =
            "INSERT INTO Tool (brandId, description, active, comments, inUse) VALUES (@brandId, @description, @active, @comments, @inUse);";

        public const string QueryUpdate =
            "UPDATE Tool SET brandId=@brandId, description=@description, active=@active, comments=@comments, inUse=@inUse WHERE Id=@Id;";

        public const string QueryDelete = "DELETE FROM Tool WHERE Id=@id;";

    }
}