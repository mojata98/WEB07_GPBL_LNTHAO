using MISA.CUKCUK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.CUKCUK.Core.Interfaces.Repository
{
    public interface IInventoryItemRepository:IBaseRepository<InventoryItem>
    {
        /// <summary>
        /// Tìm kiếm và phân trang
        /// </summary>
        /// <param name="pageIndex">STT trang</param>
        /// <param name="pageSize">Số bản ghi cần trả về</param>
        /// <param name="sort">Tiêu chí sắp xếp</param>
        /// <param name="column">Số cột trả về</param>
        /// <param name="condition">Điều kiện tìm kiếm</param>
        /// <returns>Các thông số và danh sách đối tượng được phân trang</returns>
        /// CreatedBy: LNT (03/10)
        public object GetByPaging(int pageIndex, int pageSize, string sort, string column, string condition);
        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public int Add(InventoryItem entity, List<InventoryItemUnitConvert> list);

        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: LNT (06/10)
        public string GetNewCode(string code);
    }
}
