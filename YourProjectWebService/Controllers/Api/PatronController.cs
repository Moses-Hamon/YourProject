using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using WebGrease.Css;
using YourProjectWebService.Models;

namespace YourProjectWebService.Controllers.Api
{
    public class PatronController : ApiController
    {
        private const string QuerySelectAll = "SELECT * FROM Patron";
        private const string QuerySelectOne = "SELECT * FROM Patron WHERE PatronId = @id";
        private const string QueryInsertInto =
            "INSERT INTO Patron (patronName, isGroup) VALUES (@patronName, @isGroup);";
        private const string QueryUpdate =
            "UPDATE Patron SET patronName=@patronName, isGroup=@isGroup WHERE PatronId=@PatronId;";
        private const string QueryDelete = "DELETE FROM Tool WHERE PatronId=@PatronId;";


        // GET: api/Patron
        [HttpGet]
        public IHttpActionResult GetAllPatrons()
        {
            using (var db = Database.GetConnection().OpenAndReturn())
            {
                return Ok(db.Query<Patron>(QuerySelectAll).ToList());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Patron/5
        [HttpGet]
        public IHttpActionResult GetPatron(int id)
        {
            using (var db = Database.GetConnection().OpenAndReturn())
            {
                var param = new { id };
                var patron = db.QuerySingleOrDefault<Patron>(QuerySelectOne, param);
                if (patron != null)
                {
                    return Ok(patron);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        // POST: api/Patron
        [HttpPost]
        public IHttpActionResult CreatePatron(Patron patron)
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
                    var results = db.Execute(QueryInsertInto, patron, trans);
                    if (db.LastInsertRowId > 0)
                    {
                        patron.PatronId = db.LastInsertRowId;
                    }

                    trans.Commit();
                }
                catch
                {

                    trans.Rollback();
                    return BadRequest();
                }
                db.Close();

                return Ok(patron);

            }
        }
        /// <summary>
        /// Updates a Patron entry in database using an existing patron object.
        /// </summary>
        /// <param name="patron">Patron Object</param>
        /// <returns>Patron with message</returns>
        
        // PUT: api/Patron/5
        [HttpPut]
        public IHttpActionResult UpdatePatron(Patron patron)
        {
            using (var db = Database.GetConnection().OpenAndReturn())
            using (var trans = db.BeginTransaction())
            {
                try
                {
                    var results = db.Execute(QueryUpdate, patron, trans);
                    if (results == 0)
                    {
                        return BadRequest("The Patron could not be created.");
                    }
                    trans.Commit();
                    return Ok(patron);
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    return BadRequest($"The Patron could not be created. Exception: {e.Message}");
                }
            }

        }

        // DELETE: api/Patron/5
        [HttpDelete]
        public IHttpActionResult DeletePatron(Patron patron)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            using (var db = Database.GetConnection().OpenAndReturn())
            using (var trans = db.BeginTransaction())
            {
                try
                {
                    var results = db.Execute(QueryDelete, patron, trans);
                    if (results > 0)
                    {
                        return Created(new Uri(Request.RequestUri + $"/{patron.PatronId}"), results);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return BadRequest($"Patron could not be deleted: {e.Message}");
                }
            }
        }
    }
}
