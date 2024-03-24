
using System.ComponentModel;
using LinqToDB.Mapping;

namespace Greeny.Dal.Models
{
    [Table(Name = "product_card_template", Schema = GreenySchema.SchemaName)]
    [Description("Шаблон карточки продукта")]
    public class ProductCardTemplateDataModel
    {
        [PrimaryKey, Identity]
        [Column(Name = "id", CanBeNull = false)]
        [Description("Id")]
        public virtual long Id { get; set; }

        [NotNull]
        [Column(Name = "category_id", CanBeNull = false)]
        [Description("Идентификатор категории")]
        public virtual long CategoryId { get; set; }

        [NotNull]
        [Column(Name = "name", CanBeNull = false)]
        [Description("Название продукта")]
        public virtual string Name { get; set; }

        [NotNull]
        [Column(Name = "min_order", CanBeNull = false)]
        [Description("Минимальное количество продукта")]
        public virtual double MinOrder { get; set; }

        [NotNull]
        [Column(Name = "step", CanBeNull = false)]
        [Description("Шаг изменения количества заказа")]
        public virtual double Step { get; set; }

        [NotNull]
        [Column(Name = "measure", CanBeNull = false)]
        [Description("Единица измерения")]
        public virtual int Measure { get; set; }

        [NotNull]
        [Column(Name = "image", CanBeNull = false)]
        [Description("Изображение по умолчанию")]
        public virtual byte[] Image { get; set; }
    }
}
