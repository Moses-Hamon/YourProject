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
        private const string QuerySelectOne = "SELECT * FROM Tool WHERE PatronId = @id";
        private const string QueryInsertInto =
            "INSERT INTO Patron (patronName, isGroup) VALUES (@patronName, @isGroup);";
        private const string QueryUpdate =
            "UPDATE Patron SET patronName=@patronName, isGroup=@isGroup WHERE PatronId=@PatronId;";
        private const string QueryDelete = "DELETE FROM Tool WHERE PatronId=@PatronId;";


        // GET: api/Patron
        [HttpGet]
        public IEnumerable<Patron> GetAllPatrons()
        {
            using (var db = Database.GetConnection().OpenAndReturn())
            {
                return db.Query<Patron>(QuerySelectAll).ToList();
            }
        }

        // GET: api/Patron/5
        [HttpGet]
        public HttpResponseMessage GetPatron(int id)
        {
            using (var db = Database.GetConnection().OpenAndReturn())
            {
                var param = new { id };
                var patron = db.QuerySingleOrDefault<Patron>(QuerySelectOne, param);
                if (patron != null)
                {
                    return Request.CreateResponse(HttpStatusCode.Found, patron);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        $"The Patron with id: {id} was not found!!");
                }
            }
        }

        // POST: api/Patron
        [HttpPost]
        public HttpResponseMessage CreatePatron(Patron patron)
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
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                db.Close();
                return Request.CreateResponse(HttpStatusCode.OK, patron);

            }
        }

        // PUT: api/Patron/5
        [HttpPut]
        public HttpResponseMessage UpdatePatron(Patron patron)
        {
            using (var db = Database.GetConnection().OpenAndReturn())
            using (var trans = db.BeginTransaction())
            {
                try
                {
                    var results = db.Execute(QueryUpdate, patron, trans);
                    if (results == 0)
                    {
                        throw new HttpResponseException(HttpStatusCode.BadRequest);
                    }
                    trans.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, patron);
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $" Updating Failed {e.Message}");
                }
            }

        }

        // DELETE: api/Patron/5
        [HttpDelete]
        public HttpResponseMessage DeletePatron(Patron patron)
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
                    var results = db.Execute(QueryDelete, patron, trans);
                    if (results > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                            $"Patron with Id: {patron.PatronId} has been deleted!!");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            $"Patron with id: {patron.PatronId} could not be deleted");
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
