using MISA.CUKCUK.Core.Enums;
using MISA.CUKCUK.Core.ResourcesCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.CUKCUK.Core.Entities
{
    public class ServiceResult
    {
        #region Property
        /// <summary>
        /// Mã kết quả
        /// CreatedBy: LNT (30/09)
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Thông báo cho dev
        /// CreatedBy: LNT (30/09)
        /// </summary>
        public string UserMessage { get; set; }

        /// <summary>
        /// Message trả về cho client
        /// CreatedBy: LNT (30/09)
        /// </summary>
        public List<string> DevMessage { get; set; }

        /// <summary>
        /// Dữ liệu trả về
        /// CreatedBy: LNT (30/09)
        /// </summary>
        public object Data { get; set; }

        public int rowEffects { get; set; }
        #endregion

        #region Contructor
        public ServiceResult()
        {
            this.DevMessage = new List<string>();
            this.UserMessage = null;
            this.StatusCode = (int)EnumStatusCode.Success;
            this.Data = null;
            this.rowEffects = 0;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gán log thực hiện thành công
        /// </summary>
        /// <param name="serviceResult"></param>
        /// <param name="data"></param>
        /// CreatedBy: LNT (30/09)
        /// <summary>
        public void SetSuccess(ServiceResult serviceResult, object data)
        {           
            serviceResult.DevMessage.Add(DevResourceVN.Success);
            serviceResult.UserMessage = UserResourceVN.Success;
            serviceResult.StatusCode = (int)EnumStatusCode.Success;
            serviceResult.Data = data;
        }

        /// <summary>
        /// Gán log lỗi máy chủ
        /// </summary>
        /// <param name="serviceResult"></param>
        /// CreatedBy: LNT (30/09)
        public void SetInternalServerError(ServiceResult serviceResult)
        {
            serviceResult.DevMessage.Add(DevResourceVN.InternalServerError);
            serviceResult.UserMessage = UserResourceVN.Error_Message;
            serviceResult.StatusCode = (int)EnumStatusCode.InternalServerError;
            serviceResult.Data = null;
        }
        /// <summary>
        /// Gán log lỗi dữ liệu
        /// </summary>
        /// <param name="serviceResult"></param>
        /// CreatedBy: LNT (30/09)
        public void SetBadRequest(ServiceResult serviceResult)
        {
            serviceResult.DevMessage.Add(DevResourceVN.Invalid_Input_Data);
            serviceResult.UserMessage = UserResourceVN.Error_Message;
            serviceResult.StatusCode = (int)EnumStatusCode.BadRequest;
            serviceResult.Data = null;
        }

        /// <summary>
        /// Gán log thực hiện thất bại
        /// </summary>
        /// <param name="serviceResult"></param>
        /// CreatedBy: LNT (30/09)
        public void SetNoContent(ServiceResult serviceResult)
        {
            serviceResult.DevMessage.Add(DevResourceVN.Failed);
            serviceResult.UserMessage = UserResourceVN.Failed;
            serviceResult.StatusCode = (int)EnumStatusCode.NoContent;
            serviceResult.Data = null;
        }
        #endregion
    }
}
