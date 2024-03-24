namespace Greeny.Common.Models
{
    /// <summary>
    /// Позиция заказа
    /// </summary>
    public sealed class PositionModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        public long ProductId { get; set; }

        /// <summary>
        /// Количество заказанного продукта
        /// </summary>
        public double Quntity { get; set; }

        /// <summary>
        /// Минимальная цена. Предварительно посчитанная
        /// </summary>
        public double PriceMin { get; set; }

        /// <summary>
        /// Максимальная цена.
        /// </summary>
        public double PriceMax { get; set;}

        /// <summary>
        /// Цена доставки
        /// </summary>
        public double PriceDriver { get; set; }

        /// <summary>
        /// Конечная цена
        /// </summary>
        public double FinalOrderPrice { get; set; }
    }
}
