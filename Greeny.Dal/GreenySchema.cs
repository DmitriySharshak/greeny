using Greeny.Dal.Models;
using LinqToDB;

namespace Greeny.Dal
{
    public class GreenySchema : DbManagerBase
    {
        public const string SchemaName = "greeny";

        public GreenySchema(IServiceProvider serviceCollection)
            : base(serviceCollection)
        {

        }

        public string Schema => GreenySchema.SchemaName;

        public ITable<UserDataModel> Message
        {
            get { return GetTable<UserDataModel>(); }
        }

        public ITable<AppSettingsDataModel> AppSettings
        {
            get { return GetTable<AppSettingsDataModel>(); }
        }
    }
}
