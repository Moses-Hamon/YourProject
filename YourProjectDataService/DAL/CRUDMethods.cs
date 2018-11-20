using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using YourProjectDataService.Model;

namespace YourProjectDataService.DAL
{
    public class CRUDMethods
    {

        public List<T> GetAll<T>(string QuerySelectAll)
        {
            //opens connection and returns it
            using (var db = Database.GetConnection().OpenAndReturn())
            {
                //returns a list of tools
                return db.Query<T>(QuerySelectAll).ToList();
            }
        }

        public  T GetSingle<T>(int id, string QuerySelectOne)
        {
            //opens the database
            using (var db = Database.GetConnection().OpenAndReturn())
            {
                // params allows for more control over the variables used in SQL queries
                // less likely to suffer from SQL injection
                var param = new {id};

                // executing the query and casting it into a Tool object.
                var obj = db.QuerySingleOrDefault<T>(QuerySelectOne, param);

                // check to see if the object is null and if so created an error response
                if (obj == null)
                {
                    return default(T);
                }

                // returns the requested object.
                return obj;
            }
        }

        public   T Create<T>(T obj, string QueryInsertInto) where T : IObjectIdentifier
        {
            // Opens connection
            // Executes query in a transaction just in-case rollback is required.
            using (var db = Database.GetConnection().OpenAndReturn())
            using (var trans = db.BeginTransaction())
            {
                try
                {
                    // executes with tool object
                    var results = db.Execute(QueryInsertInto, obj, trans);
                    // updates the object with the new id and returns it
                    if (db.LastInsertRowId > 0)
                    {
                        obj.Id = (int) db.LastInsertRowId;
                    }

                    //commits the transaction
                    trans.Commit();
                }
                catch
                {
                    //rolls transaction back if there is an error
                    trans.Rollback();
                }
                finally // closes connection to database
                {
                    db.Close();
                }
            }

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Object to be Updated</typeparam>
        /// <param name="obj"></param>
        /// <param name="QueryUpdate"></param>
        /// <returns></returns>
        public  T Update<T>(T obj, string QueryUpdate) where T : IObjectIdentifier
        {
            //open database connection and start transaction
            using (var db = Database.GetConnection().OpenAndReturn())
            using (var trans = db.BeginTransaction())
            {
                try
                {
                    // execute query
                    var results = db.Execute(QueryUpdate, obj, trans);
                    // if the item has been inserted then change id to match the new id from database
                    if (results == 0)
                    {
                        Console.WriteLine($"The item with {obj.Id} has not been updated");
                    }

                    // commit transaction
                    trans.Commit();
                }
                catch
                {
                    // rollback transaction if something goes wrong.
                    trans.Rollback();
                }
            }

            return obj;
        }

        public  bool Delete<T>(int id, string QueryDelete) where T : IObjectIdentifier
        {
            using (var db = Database.GetConnection().OpenAndReturn())
            using (var trans = db.BeginTransaction())
            {
                try
                {
                    var param = new {id};
                    // Executes the query for deletion
                    var results = db.Execute(QueryDelete, param, trans);
                    // SQLite will return the number of rows that have been deleted
                    if (results > 0)
                    {
                        trans.Commit();
                        return true;
                    }
                    else
                    {
                        trans.Rollback();
                        return false;
                    }
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    Console.WriteLine("File was not deleted" + e.Message);
                    return false;
                }

            }
        }

        

    }
}