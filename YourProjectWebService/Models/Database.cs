using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Web;
using Dapper;

namespace YourProjectWebService.Models
{
    public static class Database
    {
        private const string DbName = "your_project.db";
        private static readonly string ConnectionString;

        static Database()
        {
            //Gets the path for the database
            var dbPath = HttpContext.Current.Server.MapPath($"~/App_Data/{DbName}");
            ConnectionString = $"Data Source={dbPath}";
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

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