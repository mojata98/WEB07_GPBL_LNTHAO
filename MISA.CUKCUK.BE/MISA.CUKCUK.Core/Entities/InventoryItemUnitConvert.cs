using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.CUKCUK.Core.Entities
{
    public class InventoryItemUnitConvert:Base
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        [NotMap]
        [Name("ID đơn vị chuyển đổi")]
        public Guid InventoryItemUnitConvertId { get; set; }
        /// <summary>
        /// Khóa ngoại Mã nguyên vật liệu
        /// </summary>
        [NotMap]
        public Guid InventoryItemId { get; set; }
        /// <summary>
        /// Khóa ngoại đơn vị
        /// </summary>
        public Guid UnitId { get; set; }
        /// <summary>
        /// Phép tính
        /// </summary>
        public int? Calculation { get; set; }

        /// <summary>
        /// Đơn vị chuyển đổi
        /// </summary>
        public int? CalculationRate { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
    }
}
