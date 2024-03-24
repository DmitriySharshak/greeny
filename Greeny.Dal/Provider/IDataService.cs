using System.Data.Common;

namespace Greeny.Dal.Provider
{
    public interface IDataService
    {
        DbConnection CreateDbConnection();
    }
}
