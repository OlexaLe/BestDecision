using BestDecision.DataModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestDecision.Services
{
    class DatabaseManager
    {
        internal static async Task CreateDatabaseAsync(SQLiteAsyncConnection connection)
        {
            await connection.CreateTableAsync<Decision>();
            await connection.CreateTableAsync<Criteria>();
            await connection.CreateTableAsync<Variant>();
            await connection.CreateTableAsync<Rating>();
        }
    }
}
