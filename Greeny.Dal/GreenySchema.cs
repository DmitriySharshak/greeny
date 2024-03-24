using Greeny.Dal.Models;
using Greeny.Dal.Provider;
using LinqToDB;

namespace Greeny.Dal
{
    public class GreenySchema : DbManagerBase
    {
        public const string SchemaName = "greeny";

        public GreenySchema(IDataService dataService, bool newConnection = false)
            : base(dataService, newConnection)
        {

        }

        public override string Name => SchemaName;

        public ITable<UserDataModel> Users { get { return GetTable<UserDataModel>(); } }
        public ITable<CategoryDataModel> Category { get { return GetTable<CategoryDataModel>(); } }
    }
}
