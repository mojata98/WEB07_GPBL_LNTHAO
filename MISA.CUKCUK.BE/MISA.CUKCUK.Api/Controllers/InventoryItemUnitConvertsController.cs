using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Enums;
using MISA.CUKCUK.Core.Interfaces.Repository;
using MISA.CUKCUK.Core.Interfaces.Service;
using MISA.CUKCUK.Core.ResourcesCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InventoryItemUnitConvertsController : ControllerBase
    {
        #region Declare
        private readonly IInventoryItemUnitConvertService _inventoryItemUnitConvertService;
        IInventoryItemUnitConvertRepository _inventoryItemUnitConvertRepository;
        ServiceResult serviceResult;
        #endregion

        #region Contructor
        public InventoryItemUnitConvertsController(IInventoryItemUnitConvertService inventoryItemUnitConvertService, IInventoryItemUnitConvertRepository inventoryItemUnitConvertRepository)
        {
            this.serviceResult = new ServiceResult();
            this._inventoryItemUnitConvertRepository = inventoryItemUnitConvertRepository;
            this._inventoryItemUnitConvertService = inventoryItemUnitConvertService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy thông tin đơn vị chuyển đổi theo id
        /// </summary>
        /// <param name="id">ID nguyên vật liệu</param>
        /// <returns></returns>
        /// CreatedBy : LNT (27/8)
        [HttpGet("{id}")]
        public virtual IActionResult GetById(Guid id)
        {
            try
            {
                serviceResult = _inventoryItemUnitConvertService.GetById(id);
            }
            catch (Exception ex)
            {
                serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception, ex.Message));
                serviceResult.UserMessage = UserResourceVN.Error_Message;
                serviceResult.StatusCode = (int)EnumStatusCode.InternalServerError;
                return StatusCode((int)EnumStatusCode.InternalServerError, serviceResult);
            }
            return StatusCode(serviceResult.StatusCode, serviceResult);
        }
        #endregion
    }
}
