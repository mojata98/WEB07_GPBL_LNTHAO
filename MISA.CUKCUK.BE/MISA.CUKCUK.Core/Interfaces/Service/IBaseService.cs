using MISA.CUKCUK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.CUKCUK.Core.Interfaces.Service
{
    public interface IBaseService<TEntity>
    {
        /// <summary>
        /// Xử lý nghiệp vụ việc thêm mới 1 đối tượng vào db
        /// </summary>
        /// <param name="entity">Đối tượng truyền vào</param>
        /// <returns>ServiceResult - lưu trạng thái kết quả sau khi xử lý nghiệp vụ và thao tác với db </returns>
        /// CreatedBy: LNT (27/08)
        public ServiceResult Add(TEntity entity);

        /// <summary>
        /// Xử lý nghiệp vụ việc sửa thông tin 1 đối tượng vào db
        /// </summary>
        /// <param name="entity">Đối tượng truyền vào</param>
        /// <param name="entityId">Id của đối tượng truyền vào</param>
        /// <returns>ServiceResult - lưu trạng thái kết quả sau khi xử lý nghiệp vụ và thao tác với db </returns>
        /// CreatedBy: LNT (27/08)
        public ServiceResult Update(TEntity entity, Guid entityId);

        /// <summary>
        /// Lấy ra tất cả bản ghi
        /// </summary>
        /// <returns>các bản ghi</returns>
        /// CreatedBy : LNT (27/8)
        public ServiceResult GetAll();

        /// <summary>
        /// Lấy ra các bản ghi 
        /// </summary>
        /// <param name="id">id bản ghi cần lấy</param>
        /// <returns>Bản ghi có id</returns>
        /// CreatedBy : LNT (27/8)
        public ServiceResult GetById(Guid id);

        /// <summary>
        /// Xóa một bản ghi
        /// </summary>
        /// <param name="id">id bảng ghi cần xóa</param>
        /// <returns>Số hàng bị ảnh hưởng</returns>
        /// CreatedBy : LNT (27/8)
        public ServiceResult Delete(Guid id);

        /// <summary>
        /// Lấy bản ghi có cột và giá trị truyền vào
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// CreatedBy : LNT (27/8)
        public ServiceResult GetByProperty(string propertyName, string propertyValue);


    }
}
