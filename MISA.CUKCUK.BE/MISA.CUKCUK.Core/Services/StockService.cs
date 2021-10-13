using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Interfaces.Repository;
using MISA.CUKCUK.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.CUKCUK.Core.Services
{
    public class StockService : BaseService<Stock>, IStockService
    {
        #region Declare
        ServiceResult _serviceResult;
        IStockRepository _stockRepository;
        #endregion

        #region Constructor
        public StockService(IStockRepository stockRepository) : base(stockRepository)
        {
            _serviceResult = new ServiceResult();
            _stockRepository = stockRepository;
        }
        #endregion
    }
}
