using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Interfaces.Repository;
using MISA.CUKCUK.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.CUKCUK.Core.Services
{
    public class InventoryItemUnitConvertService : BaseService<InventoryItemUnitConvert>, IInventoryItemUnitConvertService
    {
        #region Declare
        ServiceResult _serviceResult;
        IInventoryItemUnitConvertRepository _inventoryItemUnitConvertRepository;
        #endregion

        #region Constructor
        public InventoryItemUnitConvertService(IInventoryItemUnitConvertRepository inventoryItemUnitConvertRepository) : base(inventoryItemUnitConvertRepository)
        {
            _serviceResult = new ServiceResult();
            _inventoryItemUnitConvertRepository = inventoryItemUnitConvertRepository;
        }
        #endregion
    }
}
