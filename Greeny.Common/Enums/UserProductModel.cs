using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greeny.Common.Enums
{
    public sealed class FarmerProductModel
    {
        public long Id { get; set; }
        public long FarmerId { get; set; }
        public long ProductId { get; set; }

        /// <summary>
        /// Минимальная цена за единицу веса (из карточки продукта)
        /// </summary>
        public double PriceMin { get; set; }

        /// <summary>
        /// Максимальная цена
        /// </summary>
        public double PriceMax { get; set; }

        /// <summary>
        /// Изображения
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// Описание продукта
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Отзывы
        /// </summary>
        public long[] FeedBackIds { get; set; }

        /// <summary>
        /// Рейтинг
        /// Динамический за год 
        /// </summary>
        public int Rate { get; set; }
    }
}
