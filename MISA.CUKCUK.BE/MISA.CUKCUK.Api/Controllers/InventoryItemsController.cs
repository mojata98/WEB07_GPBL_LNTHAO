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
    public class InventoryItemsController : ControllerBase
    {
        #region Declare
        private readonly IInventoryItemService _inventoryItemService;
        IInventoryItemRepository _inventoryItemRepository;
        ServiceResult serviceResult;
        #endregion

        #region Constructor
        public InventoryItemsController(IInventoryItemService inventoryItemService, IInventoryItemRepository inventoryItemRepository)
        {
            this._inventoryItemService = inventoryItemService;
            this.serviceResult = new ServiceResult();
            this._inventoryItemRepository = inventoryItemRepository;
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
                serviceResult = _inventoryItemService.GetAll();
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
        /// Lấy nhân viên theo id
        /// </summary>
        /// <param name="id">Id nhân viên</param>
        /// <returns></returns>
        /// CreatedBy : LNT (27/8)
        [HttpGet("{id}")]
        public virtual IActionResult GetById(Guid id)
        {
            try
            {
                serviceResult = _inventoryItemService.GetById(id);
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
        /// Xóa một đối tượng theo id
        /// </summary>
        /// <param name="id">Id cần xóa</param>
        /// <returns></returns>
        /// CreatedBy NT (27/8)
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                serviceResult = _inventoryItemService.Delete(id);
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
        /// Tìm kiếm và phân trang
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="column"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        /// CreatedBy: LNT (03/10)
        [HttpGet("Filter")]
        public IActionResult GetPaging(int pageIndex, int pageSize, string sort, string column, string condition)
        {
            try
            {
                var resultFilter = _inventoryItemRepository.GetByPaging(pageIndex, pageSize, sort, column, condition);
                if (resultFilter != null)
                {
                    return StatusCode((int)EnumStatusCode.Success, resultFilter);
                }
                else
                {
                    var msg = new
                    {
                        userMsg = UserResourceVN.No_Data,
                    };
                    return StatusCode((int)EnumStatusCode.NoContent, msg);
                }
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    devMsg = ex.Message,
                    userMsg = UserResourceVN.Error_Message,
                };
                return StatusCode((int)EnumStatusCode.InternalServerError, msg);
            }
        }

        /// <summary>
        /// Thêm mới một nhân viên
        /// </summary>
        /// <param name="employee">Thông tin nhân vien</param>
        /// <returns></returns>
        /// CreatedBy : LNT(27/8)
        [HttpPost]
        public IActionResult Add([FromBody] InventoryItem inventoryItem)
        {
            try
            {
                serviceResult = _inventoryItemService.Add(inventoryItem);
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
        /// Cập nhật một nhân viên theo id
        /// </summary>
        /// <param name="id">Id nhân viên</param>
        /// <param name="employee">Thông tin cập nhật nhân viên</param>
        /// <returns></returns>
        /// CreatedBy : LNT(27/8)
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] InventoryItem inventoryItem)
        {
            try
            {
                serviceResult = _inventoryItemService.Update(inventoryItem, id); ;
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
        /// Tạo mã nhân viên mưới
        /// </summary>
        /// <returns></returns>
        /// CreatedBy : LNT(06/10)
        [HttpGet]
        [Route("NewCode")]
        public IActionResult GetNewCode(string code)
        {
            try
            {
                serviceResult = _inventoryItemService.GetNewCode(code);
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
