using Greeny.Dal.Models;
using LinqToDB;

namespace Greeny.Dal
{
    public class GreenySchema : DbManagerBase
    {
        public const string SchemaName = "greeny";

        public GreenySchema(IDataService dataService)
            : base(dataService)
        {

        }

        public string Schema => GreenySchema.SchemaName;

        public ITable<UserDataModel> Users
        {
            get { return GetTable<UserDataModel>(); }
        }

        public ITable<CategoryDataModel> Category
        {
            get { return GetTable<CategoryDataModel>(); }
        }

        public ITable<AppSettingsDataModel> AppSettings
        {
            get { return GetTable<AppSettingsDataModel>(); }
        }
    }
}
