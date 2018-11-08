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
        // Query Strings including parameters
        private const string QuerySelectAll = "SELECT * FROM Tool";
        private const string QuerySelectOne = "SELECT * FROM Tool WHERE ToolId = @id";
        private const string QueryInsertInto =
            "INSERT INTO Tool (brandId, description, active, comments, inUse) VALUES (@brandId, @description, @active, @comments, @inUse);";
        private const string QueryUpdate =
            "UPDATE Tool SET brandId=@brandId, description=@description, active=@active, comments=@comments, inUse=@inUse WHERE ToolId=@ToolId;";
        private const string QueryDelete = "DELETE FROM Tool WHERE ToolId=@ToolId;";

  
        // GET /api/tool
        [HttpGet]
        public IEnumerable<Tool> GetAllTools()
        {
            using (var db = Database.GetConnection().OpenAndReturn())
            {
                return db.Query<Tool>(QuerySelectAll).ToList();
            }

        }

       

        // GET /api/tool/1
        [HttpGet]
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

        [HttpPost]
        public Tool CreateTool(Tool tool)
        {
            // checks to see if the model is a valid tool object
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            // Opens connection
            // Executes query in a transaction just in-case rollback is required.
            using (var db = Database.GetConnection().OpenAndReturn())
            using (var trans = db.BeginTransaction())
            {
                try
                {
                    // executes with tool object
                    var results = db.Execute(QueryInsertInto, tool, trans);

                    // updates the object with the new id and returns it
                    if (db.LastInsertRowId > 0)
                    {
                        tool.ToolId = db.LastInsertRowId;
                    }
                    //commits the transaction
                    trans.Commit();
                }
                catch
                {
                    //rolls transaction back if there is an error
                    trans.Rollback();
                }
                // closes connection to database
                db.Close();
                return tool;
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateTool(Tool tool)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            using (var db = Database.GetConnection().OpenAndReturn())
            using (var trans = db.BeginTransaction())
            {
                try
                {
                    var results = db.Execute(QueryUpdate, tool, trans);

                    if (results == 0)
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                    trans.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, tool);

                }
                catch (Exception e)
                {
                    trans.Rollback();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $" Updating Failed {e.Message}");
                }
            }
        }

        // DELETE /api/tool
        [HttpDelete]
        public HttpResponseMessage DeleteTool(Tool tool)
        {
            if (!ModelState.IsValid)
            {
               throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            using (var db = Database.GetConnection().OpenAndReturn())
                using (var trans = db.BeginTransaction())
            {
                try
                {
                    // Executes the query for deletion
                    var results = db.Execute(QueryDelete, tool, trans);
                    // SQLite will return the number of rows that have been deleted
                    if (results > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                            $"Tool with tool id: {tool.ToolId} was successfully deleted");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            $"Tool with tool id: {tool.ToolId} could not be deleted");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

            }


        }

    }
}
