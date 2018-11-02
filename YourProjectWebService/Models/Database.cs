using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

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

        public static SQLiteConnection SqLiteConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }


    }
}