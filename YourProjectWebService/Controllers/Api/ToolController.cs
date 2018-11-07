using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Dapper;
using YourProjectWebService.Models;

namespace YourProjectWebService.Controllers.Api
{
    public class ToolController : ApiController
    {
        private const string QuerySelectAll = "SELECT * FROM Tool";
        private const string QuerySelectOne = "SELECT * FROM Tool WHERE ToolId = @id";
        private const string QueryInsertInto =
            "INSERT INTO Tool (brandId, description, active, comments, inUse) VALUES (@brandId, @description, @active, @comments, @inUse);";
        private const string QueryUpdate =
            "UPDATE Tool SET brandId=@brandId, description=@description, active=@active, comments=@comments, inUse=@inUse WHERE ToolId=@id;";

        // GET /api/tool
        [System.Web.Http.HttpGet]
        public IEnumerable<Tool> GetAllTools()
        {
            using (var db = Database.GetConnection().OpenAndReturn())
            {
                return db.Query<Tool>(QuerySelectAll).ToList();
            }

        }

        // GET /api/tool/1
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetTool(int id)
        {
            //opens the database
            using (var db = Database.GetConnection().OpenAndReturn())
            {

                // params allows for more control over the variables used in SQL queries
                // less likely to suffer from SQL injection
                var param = new { id };

                // executing the query and casting it into a Tool object.
                var tool = db.QuerySingleOrDefault<Tool>(QuerySelectOne, param);

                // check to see if the object is null and if so created an error response
                if (tool == null)
                {
                    var errorMessage = $"There are no tools with id = {id}";
                    var httpError = new HttpError(errorMessage);
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, httpError);
                }
                // returns the requested object.
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, tool);
                }
                
            }
        }



    }
}
