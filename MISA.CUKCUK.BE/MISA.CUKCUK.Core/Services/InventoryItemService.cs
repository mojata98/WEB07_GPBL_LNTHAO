using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Enums;
using MISA.CUKCUK.Core.Interfaces.Repository;
using MISA.CUKCUK.Core.Interfaces.Service;
using MISA.CUKCUK.Core.ResourcesCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MISA.CUKCUK.Core.Services
{
    public class InventoryItemService : BaseService<InventoryItem>, IInventoryItemService
    {
        IInventoryItemRepository _inventoryItemRepository;

        //InventoryItemRepository inventoryItemRepository1;

        #region Constructor
        public InventoryItemService(IInventoryItemRepository inventoryItemRepository) : base(inventoryItemRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
        }

        public ServiceResult Add(InventoryItem entity, List<InventoryItemUnitConvert> list)
        {
            ServiceResult serviceResult = CheckValidate(entity);
            
            if (serviceResult.StatusCode == (int)EnumStatusCode.Success)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    serviceResult = CheckValidate(list[i]);
                    if (serviceResult.StatusCode != (int)EnumStatusCode.Success)
                    {
                        return serviceResult;
                    }
                }
                try
                {
                    int rowEffects = _inventoryItemRepository.Add(entity, list);
                    if (rowEffects > 0)
                    {
                        serviceResult.SetSuccess(serviceResult, rowEffects);
                    }
                    else
                    {
                        serviceResult.SetNoContent(serviceResult);
                    }
                }
                catch (Exception ex)
                {
                    serviceResult.SetInternalServerError(serviceResult);
                    serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception, ex.Message));
                }

            }
            return serviceResult;
        }
        #endregion
        /// <summary>
        /// Xóa thông tin theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: LNT (04/10)
        public override ServiceResult Delete(Guid id)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                //int rowEffects = _baseRepository.Delete(id);
                serviceResult = _inventoryItemRepository.Delete(id);
                if (serviceResult.rowEffects > 0)
                {
                    serviceResult.SetSuccess(serviceResult, serviceResult.Data);
                }
                else
                {
                    serviceResult.SetBadRequest(serviceResult);
                    serviceResult.DevMessage.Add(string.Format(DevResourceVN.Fail_Delete_ID, id.ToString()));
                }
            }
            catch (Exception ex)
            {
                serviceResult.SetInternalServerError(serviceResult);
                serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception, ex.Message));
            }
            return serviceResult;
        }
        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="inventoryItem"></param>
        /// <returns></returns>
        /// CreatedBy: LNt (05/10)
        public override ServiceResult Add(InventoryItem inventoryItem)
        {
            ServiceResult serviceResult = CheckValidate(inventoryItem);
            if (serviceResult.StatusCode == (int)EnumStatusCode.Success)
            {
                try
                {
                    int rowEffects = _inventoryItemRepository.Add(inventoryItem);
                    if (rowEffects > 0)
                    {
                        serviceResult.SetSuccess(serviceResult, rowEffects);
                    }
                    else
                    {
                        serviceResult.SetNoContent(serviceResult);
                    }
                }
                catch (Exception ex)
                {
                    serviceResult.SetInternalServerError(serviceResult);
                    serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception, ex.Message));
                }

            }
            return serviceResult;
        }
        /// <summary>
        /// Cập nhật thông tin theo ID
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public override ServiceResult Update(InventoryItem entity, Guid entityId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                int rowEffects = _inventoryItemRepository.Update(entity, entityId);
                if (rowEffects > 0)
                {
                    serviceResult.SetSuccess(serviceResult, rowEffects);
                }
                else
                {
                    serviceResult.SetNoContent(serviceResult);
                }
            }
            catch (Exception ex)
            {
                serviceResult.SetInternalServerError(serviceResult);
                serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception, ex.Message));
            }
            return serviceResult;
        }

        /// <summary>
        /// Kiểm tra đầu vào dữ liệu 
        /// </summary>
        /// <param name="entity">Thông tin đối tượng </param>
        /// <returns>Thông tin sau khi validate</returns>
        /// CreatedBy : LNT(27/8)
        public ServiceResult CheckValidate(InventoryItemUnitConvert entity)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                foreach (var property in entity.GetType().GetProperties())
                {
                    var name = property.Name;
                    var value = property.GetValue(entity) == "" ? null : property.GetValue(entity);
                    if (property.IsDefined(typeof(Required), false) && IsNull(value))
                    {
                        serviceResult.StatusCode = (int)EnumStatusCode.BadRequest;
                        serviceResult.DevMessage.Add(string.Format(DevResourceVN.Required, name));
                        serviceResult.UserMessage = string.Format(UserResourceVN.Required, name);
                    }

                    if (property.IsDefined(typeof(CheckExist), false) && IsDuplication(name, (string)value))
                    {
                        serviceResult.StatusCode = (int)EnumStatusCode.BadRequest;
                        serviceResult.DevMessage.Add(string.Format(DevResourceVN.Duplication, name));
                        serviceResult.UserMessage = string.Format(UserResourceVN.Duplication, name);
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResult.SetInternalServerError(serviceResult);
                serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception, ex.Message));
            }
            return serviceResult;
        }
    
        public ServiceResult GetNewCode(string code)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                string newCode = _inventoryItemRepository.GetNewCode(code);
                if (newCode != null)
                {
                    serviceResult.SetSuccess(serviceResult, newCode);
                }
                else
                {
                    serviceResult.SetNoContent(serviceResult);
                }
            }
            catch (Exception ex)
            {
                serviceResult.SetInternalServerError(serviceResult);
                serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception, ex.Message));
            }
            return serviceResult;
        }
    }
}
