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
        public IHttpActionResult GetAllTools()
        {
            using (var db = Database.GetConnection().OpenAndReturn())
            {
                return Ok(db.Query<Tool>(QuerySelectAll).ToList());
            }
        }
        
        // GET /api/tool/1
        [HttpGet]
        public IHttpActionResult GetTool(int id)
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
                    return NotFound();
                }
                // returns the requested object.

                return Ok(tool);
            }
        }

        [HttpPost]
        public IHttpActionResult CreateTool(Tool tool)
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
                    return Ok(tool);
                }
                catch (Exception e)
                {
                    //rolls transaction back if there is an error
                    trans.Rollback();
                    return InternalServerError(e);
                }
                finally // closes connection to database
                {
                    db.Close();
                }
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateTool(Tool tool)
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
                        return NotFound();
                    }
                    trans.Commit();
                    return Ok(tool);

                }
                catch (Exception e)
                {
                    trans.Rollback();
                    return InternalServerError(e);
                }
            }
        }

        // DELETE /api/tool
        [HttpDelete]
        public IHttpActionResult DeleteTool(Tool tool)
        {
            if (!ModelState.IsValid)
            {
               return BadRequest("The object sent was not a valid object");
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
                        return Ok();
                    }

                    return NotFound();
                }
                catch (Exception e)
                {
                    
                    return InternalServerError(e);
                }

            }


        }

    }
}
