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
    public class BrandController : ApiController
    {
        private const string QuerySelectAll = "SELECT * FROM Brand";
        private const string QuerySelectOne = "SELECT * FROM Brand WHERE BrandId = @id";
        private const string QueryInsertInto =
            "INSERT INTO Brand (brandName) VALUES (@brandName);";
        private const string QueryUpdate =
            "UPDATE Brand SET brandName=@brandName WHERE BrandId=@BrandId;";
        private const string QueryDelete = "DELETE FROM Brand WHERE BrandId=@BrandId;";

        // GET: api/Brand
        public IHttpActionResult Get()
        {
            return Ok(Database.GetConnection().Query<Brand>(QuerySelectAll).ToList());

        }

        // GET: api/Brand/5
        [HttpGet]
        public IHttpActionResult GetBrand(int id)
        {
            using (var db = Database.GetConnection().OpenAndReturn())
            {
                var param = new { id };
                var brand = db.QuerySingleOrDefault<Brand>(QuerySelectOne, param);

                if (brand == null)
                {
                    return NotFound();
                }

                return Ok(brand);
            }
        }
        /// <summary>
        /// Creates a new Brand entry into the database.
        /// </summary>
        /// <param name="brand">Brand object that will be created into the database</param>
        // POST: api/Brand
        [HttpPost]
        public IHttpActionResult CreateNewBrand(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The object sent does not match, please try again");
            }
            using (var db = Database.GetConnection().OpenAndReturn())
            using (var trans = db.BeginTransaction())
            {
                try
                {
                    var results = db.Execute(QueryInsertInto, brand, trans);
                    if (db.LastInsertRowId > 0)
                        brand.BrandId = db.LastInsertRowId;
                    trans.Commit();
                    return Ok(brand);
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    return InternalServerError(e);
                }
                finally
                {
                    db.Close();
                }
            }
        }

        // PUT: api/Brand
        [HttpPut]
        public IHttpActionResult UpdateBrand(Brand brand)
        {
            // Check if the model sent to the server is the correct format.
            if (!ModelState.IsValid)
            {
                return BadRequest("The item sent was not in the correct format");
            }
            // Creates the connection and the transaction.
            using (var db = Database.GetConnection().OpenAndReturn())
            using (var trans = db.BeginTransaction())
            {
                try
                {
                    // Execute update
                    var results = db.Execute(QueryUpdate, brand, trans);
                    // Execute() returns the number of rows affected by the query
                    if (results == 0)
                    {
                        return NotFound();
                    }

                    // Commits the transaction
                    trans.Commit();
                    return Ok(brand);
                }
                // Catches any exceptions and rolls back the transaction
                // returns the relevant status code with the message.
                catch (Exception e)
                {
                    trans.Rollback();
                    return InternalServerError(e);
                }
            }
        }

        // DELETE: api/Brand/5
        public IHttpActionResult DeleteBrand(Brand brand)
        {
            using (var db = Database.GetConnection().OpenAndReturn())
            using (var trans = db.BeginTransaction())
            {
                try
                {
                    var results = db.Execute(QueryDelete, brand, trans);
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
