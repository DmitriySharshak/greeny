using LinqToDB.Mapping;
using System.ComponentModel;

namespace Greeny.Dal.Models
{
    [Table(Name = "app_settings")]
    [Description("Настройки")]
    public class AppSettingsDataModel
    {
        [PrimaryKey, Identity]
        [Column(Name = "id", CanBeNull = false)]
        [Description("Id")]
        public virtual string Id { get; set; }

        [NotNull]
        [Column(Name = "value", CanBeNull = false)]
        [Description("Значение")]
        public virtual string Value { get; set; }
    }
}
