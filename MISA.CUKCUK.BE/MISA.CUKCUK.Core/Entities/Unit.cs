using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.CUKCUK.Core.Entities
{
    public class Unit:Base
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        [NotMap]
        [Name("ID đơn vị tính")]
        public Guid UnitId { get; set; }
        /// <summary>
        /// Mã đơn vị tính
        /// </summary>
        [NotMap]
        [Name("Mã đơn vị tính")]
        public string UnitCode { get; set; }
        /// <summary>
        /// Tên đơn vị tính
        /// </summary>
        [Required]
        [CheckExist]
        [Name("Tên đơn vị tính")]
        public string UnitName { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
    }
}
