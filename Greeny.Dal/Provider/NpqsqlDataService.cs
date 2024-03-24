using Npgsql;
using System.Data.Common;

namespace Greeny.Dal.Provider
{
    public class NpqsqlDataService : IDataService
    {
        private readonly string _dbConnectionString;
        public NpqsqlDataService(string connectionString)
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
