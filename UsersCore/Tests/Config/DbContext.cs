using Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Tests.Config
{
    public class DbContext
    {
        public static DbConnection InitConnection()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            conn.Open();

            return conn;
        }

        public static AppDbContext DataBaseContext (DbConnection dbConnection)
        {
            var opc = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(dbConnection)
                .Options;

            var context = new AppDbContext(opc);
            context.Database.EnsureCreated();

            return context;
        }
    }
}
