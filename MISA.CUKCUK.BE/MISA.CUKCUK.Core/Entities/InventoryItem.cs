using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.CUKCUK.Core.Entities
{
    public class InventoryItem : Base
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        [NotMap]
        [Name("ID nguyên vật liệu")]
        public Guid InventoryItemId { get; set; }
        /// <summary>
        /// Mã nguyên vật liệu
        /// </summary>
        [Required]
        [CheckExist]
        [Name("Mã nguyên vật liệu")]
        public string InventoryItemCode { get; set; }
        /// <summary>
        /// Tên nguyên vật liệu
        /// </summary>
        [Required]
        [Name("Tên nguyên vật liệu")]
        public string InventoryItemName { get; set; }
        /// <summary>
        /// Đơn vị tính Khóa ngoại
        /// </summary>
        [Required]
        [Name("ID đơn vị tính")]
        public Guid UnitId { get; set; }
        /// <summary>
        /// Tên đơn vị
        /// </summary>
        [NotMap]
        public string UnitName { get; set; }
        /// <summary>
        /// Khóa ngoại Kho
        /// </summary>
        public Guid? StockId { get; set; }
        /// <summary>
        /// Thời lượng hạn sử dụng
        /// </summary>
        public int ExpirationNumber { get; set; }
        /// <summary>
        /// Kiểu hạn sử dụng
        /// </summary>
        public int ExpirationType { get; set; }
        /// <summary>
        /// Số lượng tồn tối thiểu
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Ngưng sử dụng
        /// </summary>
        public int StopWorking { get; set; }
        /// <summary>
        /// Nhóm nguyên vật liệu
        /// </summary>
        public string InventoryItemGroupName { get; set; }
        /// <summary>
        /// Tính chất
        /// </summary>
        public string PropertyOfInventoryItem { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List đơn vị chuyển đổi
        /// </summary>
        /// Created By : LNT (29/9)
        [NotMap]
        [CheckList]
        public List<InventoryItemUnitConvert> UnitConverts {
            //get {
            //    return new List<InventoryItemUnitConvert>();

            //}
            //set { } 
            get;
            set;
        }
    }
}
