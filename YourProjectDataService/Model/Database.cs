using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Web;
using Dapper;

namespace YourProjectDataService.Model
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

        private static string Encrypt(string data)
        {
            const string ENCRYPTION_KEY = "HJKlfjdsklafnj7843543";

            string saltedPassword = ENCRYPTION_KEY + data + ENCRYPTION_KEY;

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(saltedPassword);
            bytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(bytes);
            string hashed = System.Text.Encoding.ASCII.GetString(bytes);

            return hashed;
        }

        public static bool ValidateUserCredentials(string user, string pass)
        {
            using (var db = GetConnection())
            {
                var query = "SELECT COUNT(*) FROM Staff WHERE userName = @san AND password = @sap;";
                var param = new { san = user, sap = Encrypt(pass) };
                var results = (long)db.ExecuteScalar(query, param);
                return results > 0;
            }
        }

        public static void CreateUser(string user, string pass)
        {
            using (var db = GetConnection().OpenAndReturn())
            using (var transaction = db.BeginTransaction())
            {
                try
                {
                    var query = "INSERT INTO Staff (userName, password) VALUES (@san, @sap);";
                    var param = new { san = user, sap = Encrypt(pass) };
                    var results = db.Execute(query, param);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

    }
}
