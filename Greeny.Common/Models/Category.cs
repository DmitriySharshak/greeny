namespace Greeny.Common.Models
{
    /// <summary>
    /// Категория продукции
    /// </summary>
    public sealed class Category
    {
        /// <summary>
        /// Идентификатор категории
        /// </summary>
        public long Id { get; init; }

        /// <summary>
        /// Дочерние категории
        /// </summary>
        public IReadOnlyCollection<Category> Descendants { get; set; }

        /// <summary>
        /// Наименование категории
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Путь к файлу изображения
        /// </summary>
        public string ImagePath { get; init; }

        /// <summary>
        /// Изображение в формате Base64
        /// </summary>
        public string ImageBase64 { get; init; }

        public override bool Equals(object obj)
        {
            return obj is Category other && this.Equals(other);
        }

        public bool Equals(Category other)
        {
            return string.Equals(this.Name, other.Name, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(this.ImagePath, other.ImagePath, StringComparison.OrdinalIgnoreCase) &&
                   this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.OrdinalIgnoreCase.GetHashCode(this.Name ?? string.Empty);
                hashCode = (hashCode * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(this.ImagePath ?? string.Empty);
                hashCode = (hashCode * 397) ^ this.Id.GetHashCode();

                return hashCode;
            }
        }
    }
}
