using Gnivc.DataBus.Common.Dal.Migration;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.PostgreSQL;
using LinqToDB.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Greeny.Dal
{
    // TODO: Почему то попытка использовать существующий PostgreSQLDataProvider93 из linq2db приводит к ошибке
    // Error CS0122  'PostgreSQLDataProvider93' is inaccessible due to its protection level
    // Возможно это из за кэша внутри студии. Пока обошел, создав свой класс
    class PostgreSQLDataProvider93 : PostgreSQLDataProvider { public PostgreSQLDataProvider93() : base(ProviderName.PostgreSQL93, PostgreSQLVersion.v93) { } }

    public abstract class DbManagerBase : IDisposable
    {
        private static IDataProvider dataProvider = new PostgreSQLDataProvider93();
        private bool isOuterConnection = false;

        private DataConnection Connection { get; set; }

        public DbManagerBase(IServiceProvider serviceCollection, bool requreNewConnection = false)
        {
            var connection = DbConnectionManager.Instance.PeekOrNull();

            if (connection == null || requreNewConnection)
            {
                var dataService = serviceCollection.GetService<IDataService>();

                var dbConnection = dataService.CreateDbConnection();
                dbConnection.Open();

                connection = new DataConnection(dataProvider, dbConnection);

                DbConnectionManager.Instance.Push(connection);

                isOuterConnection = true;
            }
            else
            {
                isOuterConnection = false;
            }

            Connection = connection;
        }

        public void Dispose()
        {
            //logger.Debug("Dispose DbManager for operation");

            if (isOuterConnection)
            {
                //logger.Debug("Close db connection for operation");

                DbConnectionManager.Instance.Pop();

                Connection.Close();

                // DbConnection linq2db не закрывает коннекты, которые он не создавал сам.
                // Поэтому коннекты, созданные вручную, надо закрывать явно
                // https://github.com/linq2db/linq2db/blob/864899f8721ba98682c87bc9788bd438c87643a6/Source/LinqToDB/Data/DataConnection.cs#L220
                Connection.Connection.Close();
            }
        }

        // Методы для доступа к даным
        public ITable<T> GetTable<T>() where T : class
        {
            return Connection.GetTable<T>();
        }

        public IEnumerable<T> Query<T>(string sql, object parameters = null)
        {
            return Connection.Query<T>(sql, parameters);
        }

        public IEnumerable<T> Query<T>(T template, string sql, object parameters = null)
        {
            return Connection.Query(template, sql, parameters);
        }

        public int Execute(string sql, object parameters = null)
        {
            if (parameters == null)
                return Connection.Execute(sql);
            else
                return Connection.Execute(sql, parameters);
        }

        public T Execute<T>(string sql, object parameters = null)
        {
            if (parameters == null)
                return Connection.Execute<T>(sql);
            else
                return Connection.Execute<T>(sql, parameters);
        }

        public BulkCopyRowsCopied BulkCopy<T>(BulkCopyOptions options, IEnumerable<T> source) where T : class
        {
            return Connection.BulkCopy(options, source);
        }

        public void Migrate<T>()
        {
            new SchemaMigration<T>(Connection).CreateOrUpdateTable();
        }
    }
}
