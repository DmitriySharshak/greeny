using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Greeny.Common.Enums;

namespace Greeny.Common.Models
{
    /// <summary>
    /// Заказ 
    /// </summary>
    public sealed class OrderModel
    {
        /// <summary>
        /// Идентификатор 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Дата и время создания заказа
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public OrderStatus Status { get; set; }

        public List<PositionModel> Positions { get; set; }

    }
}
