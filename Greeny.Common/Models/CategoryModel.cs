using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Наименование категории
        /// </summary>
        public string Name { get; set; }
    }
}
