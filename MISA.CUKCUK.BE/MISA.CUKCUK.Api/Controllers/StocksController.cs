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
    public class StocksController : ControllerBase
    {
        #region Declare
        private readonly IStockService _stockService;
        IStockRepository _stockRepository;
        ServiceResult serviceResult;
        #endregion

        #region Contructor
        public StocksController(IStockService stockService, IStockRepository stockRepository)
        {
            this.serviceResult = new ServiceResult();
            this._stockRepository = stockRepository;
            this._stockService = stockService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy danh sách kho
        /// </summary>
        /// <returns></returns>
        /// CreatedBy : LNT (27/8)
        [HttpGet]
        public virtual IActionResult GetAll()
        {
            try
            {
                serviceResult = _stockService.GetAll();
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

        ///// <summary>
        ///// Lấy thông tin theo tên kho
        ///// </summary>
        ///// <param name="id">Mã đơn vị tính</param>
        ///// <returns></returns>
        ///// CreatedBy : LNT (27/8)
        //[HttpGet("{propertyValue}")]
        //public virtual IActionResult GetByProperty(string propertyValue)
        //{
        //    try
        //    {
        //        var propertyName = "StockName";
        //        serviceResult = _stockService.GetByProperty(propertyName, propertyValue);
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception, ex.Message));
        //        serviceResult.UserMessage = UserResourceVN.Error_Message;
        //        serviceResult.StatusCode = (int)EnumStatusCode.InternalServerError;
        //        return StatusCode((int)EnumStatusCode.InternalServerError, serviceResult);
        //    }
        //    return StatusCode(serviceResult.StatusCode, serviceResult);
        //}

        /// <summary>
        /// Thêm mới một kho
        /// </summary>
        /// <param name="employee">Thông tin kho</param>
        /// <returns></returns>
        /// CreatedBy : LNT(27/8)
        [HttpPost]
        public IActionResult Add([FromBody] Stock stock)
        {
            try
            {
                serviceResult = _stockService.Add(stock);
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
