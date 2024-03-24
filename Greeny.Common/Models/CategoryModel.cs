namespace Greeny.Common.Models
{
    /// <summary>
    /// Категория продукции
    /// </summary>
    public sealed class CategoryModel
    {
        /// <summary>
        /// Идентификатор категории
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Родительская категория
        /// </summary>
        public long? ParentId { get;set; }

        /// <summary>
        /// Наименование категории
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Изображение в формате base 64
        /// </summary>
        public string Image { get; set; }
    }
}
