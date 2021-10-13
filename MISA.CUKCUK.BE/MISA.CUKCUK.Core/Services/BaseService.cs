using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Enums;
using MISA.CUKCUK.Core.Interfaces;
using MISA.CUKCUK.Core.Interfaces.Service;
using MISA.CUKCUK.Core.ResourcesCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MISA.CUKCUK.Core.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity>
    {
        #region Declare
        protected IBaseRepository<TEntity> _baseRepository;
        // mảng chứa error
        List<string> messageArr = new List<string>();
        // check vaidate
        bool isValid = true;
        ServiceResult serviceResult;
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            this._baseRepository = baseRepository;
            this.serviceResult = new ServiceResult();
        }
        #endregion
        /// <summary>
        /// Thêm mới bản ghi vào db
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy: LNT (27/08)
        public virtual ServiceResult Add(TEntity entity)
        {
            ServiceResult serviceResult = CheckValidate(entity);
            if (serviceResult.StatusCode == (int)EnumStatusCode.Success)
            {
                try
                {
                    int rowEffects = _baseRepository.Add(entity);
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
        /// Xóa thông tin theo ID
        /// </summary>
        /// <param name="id">ID cần xem xóa</param>
        /// <returns>Bản ghi đã xóa</returns>
        /// CreatedBy: LNT (27/8)
        public virtual ServiceResult Delete(Guid id)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                //int rowEffects = _baseRepository.Delete(id);
                serviceResult = _baseRepository.Delete(id);
                if (serviceResult.rowEffects > 0)
                {
                    serviceResult.SetSuccess(serviceResult, serviceResult.Data);
                }
                else if((int)serviceResult.StatusCode == (int)EnumStatusCode.InternalServerError)
                {
                    return serviceResult;
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
        /// Lấy ra dữ liệu của toàn bộ đối tượng trong db
        /// </summary>
        /// <returns>Danh sách đối tượng</returns>
        /// CreatedBy:LNT(27/08)
        public virtual ServiceResult GetAll()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult = _baseRepository.GetAll();
                if (serviceResult.Data != null)
                {
                    serviceResult.SetSuccess(serviceResult, serviceResult.Data);
                }
                else
                {
                    serviceResult.SetInternalServerError(serviceResult);
                }

            }
            catch (Exception ex)
            {
                var msg = string.Format(DevResourceVN.Exception, ex.Message);
                serviceResult.SetInternalServerError(serviceResult);
                serviceResult.DevMessage.Add(msg);
            }
            return serviceResult;
        }
       
        /// <summary>
        /// Lấy thông tin theo ID
        /// </summary>
        /// <param name="id">ID cần xem thông tin</param>
        /// <returns>Thông tin đối thượng</returns>
        /// CreatedBy: LNT (27/8)
        public virtual ServiceResult GetById(Guid id)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var result = _baseRepository.GetById(id);

                if (result.Data != null)
                {
                    serviceResult = result;
                    serviceResult.DevMessage.Add(DevResourceVN.Success);
                    serviceResult.UserMessage = UserResourceVN.Success;
                    return serviceResult;
                }
                else
                {
                    serviceResult.SetBadRequest(serviceResult);
                    serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception, id.ToString()));
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
        /// Lấy thông tin theo cột 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        /// CreatedBy: LNt (27/08)
        public ServiceResult GetByProperty(string propertyName, string propertyValue)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var result = _baseRepository.GetByProperty(propertyName, propertyValue);

                if (result.Data != null)
                {
                    serviceResult = result;
                    serviceResult.DevMessage.Add(DevResourceVN.Success);
                    serviceResult.UserMessage = UserResourceVN.Success;
                    return serviceResult;
                }
                else
                {
                    serviceResult.SetBadRequest(serviceResult);
                    serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception, propertyValue));
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
        /// Cập nhật thông tin theo ID
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// CreatedBy: LNT (27/08)
        public virtual ServiceResult Update(TEntity entity, Guid entityId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                int rowEffects = _baseRepository.Update(entity, entityId);
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
        /// Kiểm tra giá trị null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsNull(object value)
        {
            if (string.IsNullOrEmpty(Convert.ToString(value)) || value == null) return true;
            return false;
        }

        /// <summary>
        /// Kiểm tra trùng với giá trị có trước
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsDuplication(string name, string value)
        {
            var entityDuplicate = _baseRepository.GetByProperty(name, value);
            if (entityDuplicate.Data != null) return true;
            return false;
        }

        /// <summary>
        /// Check required
        /// </summary>
        /// <param name="prop">Property của đối tượng</param>
        /// <param name="propValue"> Value của property</param>
        /// <param name="fieldName">Tên của property</param>
        /// <returns>isValid : True or False</returns>
        /// CreatedBy: LNT (27/08)
        public bool IsRequired(PropertyInfo prop, object propValue, string fieldName)
        {
            if (prop.IsDefined(typeof(Required), false))
            {
                if (string.IsNullOrEmpty(Convert.ToString(propValue)) || propValue == null)
                {
                    messageArr.Add(string.Format(DevResourceVN.Required, fieldName));
                    serviceResult.StatusCode = (int)EnumStatusCode.BadRequest;
                    serviceResult.UserMessage = UserResourceVN.Invalid_data;
                    isValid = false;
                }
            }
            return isValid;
        }

        /// <summary>
        /// Kiểm tra đầu vào dữ liệu 
        /// </summary>
        /// <param name="entity">Thông tin đối tượng </param>
        /// <returns>Thông tin sau khi validate</returns>
        /// CreatedBy : LNT(27/8)
        public ServiceResult CheckValidate(TEntity entity)
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
                        serviceResult.DevMessage.Add(string.Format(DevResourceVN.Required,name));
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
                serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception,ex.Message));
            }
            return serviceResult;
        }
    }
}
