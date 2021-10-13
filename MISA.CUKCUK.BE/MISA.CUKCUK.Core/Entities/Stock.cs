using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.CUKCUK.Core.Entities
{
    public class Stock:Base
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        [NotMap]
        [Name("ID Kho")]
        public Guid StockId { get; set; }
        /// <summary>
        /// Mã đơn vị tính
        /// </summary>
        [Required]
        [CheckExist]
        [Name("Mã Kho")]
        public string StockCode { get; set; }
        /// <summary>
        /// Tên đơn vị tính
        /// </summary>
        [Required]
        [Name("Tên kho")]
        public string StockName { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
    }
}
