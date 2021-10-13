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
    public class UnitsController : ControllerBase
    {
        #region Declare
        private readonly IUnitService _unitService;
        IUnitRepository _unitRepository;
        ServiceResult serviceResult;
        #endregion

        #region Contructor
        public UnitsController(IUnitService unitService, IUnitRepository unitRepository)
        {
            this.serviceResult = new ServiceResult();
            this._unitRepository = unitRepository;
            this._unitService = unitService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy danh sách nguyên vật liệu
        /// </summary>
        /// <returns></returns>
        /// CreatedBy : LNT (27/8)
        [HttpGet]
        public virtual IActionResult GetAll()
        {
            try
            {
                serviceResult = _unitService.GetAll();
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

        /// <summary>
        /// Lấy thông tin theo tên đơn vị tính
        /// </summary>
        /// <param name="id">Mã đơn vị tính</param>
        /// <returns></returns>
        /// CreatedBy : LNT (27/8)
        [HttpGet("{propertyValue}")]
        public virtual IActionResult GetByProperty(string propertyValue)
        {
            try
            {
                var propertyName = "UnitName";
                serviceResult = _unitService.GetByProperty(propertyName, propertyValue);
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

        /// <summary>
        /// Thêm mới một đơn vị tính
        /// </summary>
        /// <param name="employee">Thông tin đơn vị tính</param>
        /// <returns></returns>
        /// CreatedBy : LNT(27/8)
        [HttpPost]
        public IActionResult Add([FromBody] Unit unit)
        {
            try
            {
                serviceResult = _unitService.Add(unit);
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
