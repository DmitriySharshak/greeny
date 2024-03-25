using LinqToDB.Mapping;
using System.ComponentModel;

namespace Greeny.Dal.Models
{
    [Table(Name = "category", Schema = GreenySchema.SchemaName)]
    [Description("Категория товара")]
    public class CategoryDataModel
    {
        [PrimaryKey, Identity]
        [Column(Name = "id", CanBeNull = false)]
        [Description("Id")]
        public virtual long Id { get; set; }

        [Column(Name = "parent_id")]
        [Description("Родительская категория")]
        public virtual long? ParentId { get; set; }

        [NotNull]
        [Column(Name = "name", CanBeNull = false)]
        [Description("Наименование")]
        public virtual string Name { get; set; }

        [NotNull]
        [Column(Name = "image_path", CanBeNull = false)]
        [Description("Путь к файлу изображения")]
        public virtual string ImagePath { get; set; }

        [NotNull]
        [Column(Name = "show", CanBeNull = false)]
        [Description("Показывать категорию")]
        public virtual bool Show { get; set; }
    }
}
