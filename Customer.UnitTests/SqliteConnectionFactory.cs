using Customer_Union.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Union.UnitTests
{
    public class SqliteConnectionFactory : IDbConnectionFactory
    {
        private readonly IDbConnection _connection;

        public SqliteConnectionFactory()
        {
            _connection = new SqliteConnection("Data Source=:memory:;Mode=Memory;Cache=Shared");
            _connection.Open();
        }
        public IDbConnection CreateConnection()
        {
            return _connection;
        }
    }
}
