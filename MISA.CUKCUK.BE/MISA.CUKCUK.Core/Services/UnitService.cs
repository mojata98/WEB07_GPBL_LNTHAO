using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Interfaces.Repository;
using MISA.CUKCUK.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.CUKCUK.Core.Services
{
    public class UnitService : BaseService<Unit>, IUnitService
    {
        #region Declare
        ServiceResult _serviceResult;
        IUnitRepository _unitRepository;
        #endregion

        #region Constructor
        public UnitService(IUnitRepository unitRepository) : base(unitRepository)
        {
            _serviceResult = new ServiceResult();
            _unitRepository = unitRepository;
        }
        #endregion
    }
}
