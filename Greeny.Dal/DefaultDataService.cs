using Npgsql;
using System.Data.Common;

namespace Greeny.Dal
{
    public class DefaultDataService: IDataService
    {
        private readonly string _dbConnectionString;
        public DefaultDataService(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            _dbConnectionString = connectionString;
        }
        public DbConnection CreateDbConnection()
        {
            return new NpgsqlConnection(_dbConnectionString);
        }
    }
}
