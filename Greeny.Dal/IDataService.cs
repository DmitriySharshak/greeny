using System.Data.Common;

namespace Greeny.Dal
{
    public interface IDataService
    {
        DbConnection CreateDbConnection();
    }
}
