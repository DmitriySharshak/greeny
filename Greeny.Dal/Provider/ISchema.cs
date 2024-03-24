using Greeny.Dal.Models;
using LinqToDB;
using LinqToDB.Data;

namespace Greeny.Dal.Provider
{
    public interface ISchema
    {
        string Name { get; }

        ITable<AppSettingsDataModel> AppSettings { get; }

        DataConnection Connection { get; }
    }
}
