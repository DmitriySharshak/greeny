using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greeny.Common.Models
{
    public sealed class FeedBack
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }

        /// <summary>
        /// Рейтинг (1-5)
        /// </summary>
        public int Stars { get; set; }

    }
}
