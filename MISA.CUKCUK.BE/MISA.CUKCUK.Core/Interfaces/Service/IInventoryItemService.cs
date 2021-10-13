using MISA.CUKCUK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.CUKCUK.Core.Interfaces.Service
{
    public interface IInventoryItemService:IBaseService<InventoryItem>
    {
        #region Methods
        public ServiceResult Add(InventoryItem entity, List<InventoryItemUnitConvert> list);

        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: LNT (06/10)
        public ServiceResult GetNewCode(string code);
        #endregion
    }
}
