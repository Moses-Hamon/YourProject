using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Web;
using Dapper;

namespace YourProjectWebService.Models
{
    /// <summary>
    /// Database class that holds information and methods relevant to the database.
    /// </summary>
    public static class Database
    {
        private const string DbName = "your_project.db";
        private static readonly string ConnectionString;

        /// <summary>
        /// Class constructor used to map the database path.
        /// Provides information for the connection string.
        /// </summary>
        static Database()
        {
            //Gets the path for the database
            var dbPath = HttpContext.Current.Server.MapPath($"~/App_Data/{DbName}");
            ConnectionString = $"Data Source={dbPath}";
        }

        /// <summary>
        ///  Method for returning a new SQLiteConnection
        /// </summary>
        /// <returns>New SQLiteConnection </returns>
        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        /// <summary>
        /// Method for creating database using the schema.txt that is provided.
        /// </summary>
        public static void CreateDataBase()
        {
            using (var db = GetConnection())
            {
                var appDataPath = HttpContext.Current.Server.MapPath("~/App_Data");
                var scriptFileName = "schema.txt";
                var scriptPath = Path.Combine(appDataPath, scriptFileName);
                var queries = File.ReadAllText(scriptPath);
                db.Execute(queries);
            }
        }


    }
}