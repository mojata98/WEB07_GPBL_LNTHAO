using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.CUKCUK.Core.Enums
{
    public enum EnumStatusCode
    {
        /// <summary>
        /// Thao tác thành công
        /// </summary>
        Success = 200,

        /// <summary>
        /// Thêm thành công
        /// </summary>
        Created = 201,

        /// <summary>
        /// Không có dữ liệu
        /// </summary>
        NoContent = 204,

        /// <summary>
        /// Dữ liệu không hợp lệ
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// Không được cấp quyền
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// Bị cấm truy cập
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// Không tìm thấy trang
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// Lỗi bên phía server
        /// </summary>
        InternalServerError = 500
    }
}
