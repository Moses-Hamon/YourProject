using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using YourProjectWebService.Models;

namespace YourProjectWebService.Controllers.Api
{
    public class ToolController : ApiController
    {
        private const string QuerySelectAll = "SELECT * FROM Tool";
        private const string QuerySelectOne = "SELECT * FROM Tool WHERE ToolId = @id";

        private const string QueryInsertInto =
            "INSERT INTO Tool (description, active, comments, inUse) VALUES (@brandId, @description, @active, @comments, @inUse);";

        // GET /api/tool
        [HttpGet]
        public IEnumerable<Tool> GetAllTools()
        {
            using (var db = Database.GetConnection().OpenAndReturn())
            {
                return db.Query<Tool>(QuerySelectAll).ToList();
            }
                
        }

    }
}
