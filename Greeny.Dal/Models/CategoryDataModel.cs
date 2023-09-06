using LinqToDB.Mapping;
using System.ComponentModel;

namespace Greeny.Dal.Models
{
    [Table(Name = "category", Schema = GreenySchema.SchemaName)]
    [Description("")]
    public class CategoryDataModel
    {
        [PrimaryKey, Identity]
        [Column(Name = "id", CanBeNull = false)]
        [Description("Id")]
        public virtual long Id { get; set; }

        [NotNull]
        [Column(Name = "name", CanBeNull = false)]
        [Description("Наименование")]
        public virtual string Name { get; set; }
    }
}
