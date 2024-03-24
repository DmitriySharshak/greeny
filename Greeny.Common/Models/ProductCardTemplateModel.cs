using Greeny.Common.Enums;

namespace Greeny.Common.Models
{
    /// <summary>
    /// Шаблон продукта
    /// </summary>
    public sealed class ProductCardTemplateModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор категории
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// Название продукта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Минимальное количество заказа
        /// </summary>
        public double MinOrder { get; set; }

        /// <summary>
        /// Шаг изменения количества заказа
        /// </summary>
        public double Step { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public ProductMeasure Measure { get; set; }

        /// <summary>
        /// Изображение продукта default
        /// </summary>
        public byte[] Image { get; set; }
    }
}
