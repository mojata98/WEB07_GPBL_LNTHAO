using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Interfaces;
using MISA.CUKCUK.Core.ResourcesCore;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace MISA.CUKCUK.Infrastructure.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>, IDisposable
    {

        #region Declare
        IConfiguration _configuration;
        readonly string _connectionString = string.Empty;
        public IDbConnection _dbConnection = null;
        protected string _tableName;
        ServiceResult serviceResult;
        #endregion

        #region Constructor
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MISACUKCUKConnectionString");
            _dbConnection = new MySqlConnection(_connectionString);
            _tableName = typeof(TEntity).Name;
            serviceResult = new ServiceResult();
        }
        #endregion
        /// <summary>
        /// Thêm mới đối tượng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy: LNT (27/08)
        public virtual int Add(TEntity entity)
        {
            try
            {
                var rowEffects = 0;

                _dbConnection = new MySqlConnection(_connectionString);
                _dbConnection.Open();

                using (var transaction = _dbConnection.BeginTransaction())
                {
                    try
                    {
                        var parameters = MappingDbType(entity);

                        rowEffects = _dbConnection.Execute($"Proc_Insert{_tableName}", param: parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return rowEffects;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }


        }

        public virtual int Add(InventoryItem entity, List<InventoryItemUnitConvert> list)
        {
            return 1;
        }
        /// <summary>
        /// Xóa 1 đối tượng theo ID
        /// </summary>
        /// <param name="entityId">ID cần xóa</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// CreatedBy: LNT (27/08)
        public virtual ServiceResult Delete(Guid entityId)
        {
            MySqlConnection dbConnection = new MySqlConnection(_connectionString);
            serviceResult = new ServiceResult();
            dbConnection.Open();
            int rowEffects = 0;
            var transaction = dbConnection.BeginTransaction();
            //using (var transaction = _dbConnection.BeginTransaction())
            //{
            try
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add($"${_tableName}Id", entityId.ToString());
                string sqlCommand = $"Proc_Delete{_tableName}ById";
                rowEffects = _dbConnection.Execute(sqlCommand, param: parameter, commandType: CommandType.StoredProcedure);
                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                serviceResult.SetInternalServerError(serviceResult);
                serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception, ex.Message));
                return serviceResult;
            }
            finally
            {
                transaction.Dispose();
                dbConnection.Close();
                dbConnection.Dispose();
            }
            //}
            serviceResult.rowEffects = rowEffects;
            return serviceResult;
            //return 1;
        }

        public void Dispose()
        {
            this._dbConnection.Dispose();
        }
        /// <summary>
        /// Lấy ra dữ liệu của toàn bộ đối tượng trong db
        /// </summary>
        /// <returns>Danh sách đối tượng</returns>
        /// CreatedBy:LNT(27/08)
        public virtual ServiceResult GetAll()
        {
            serviceResult = new ServiceResult();
            try
            {
                string sqlCommand = $"Proc_Get{_tableName}";
                var entities = _dbConnection.Query<TEntity>(sqlCommand, commandType: CommandType.StoredProcedure);
                serviceResult.Data = entities;
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
        /// Lấy ra dữ liệu của bộ đối tượng trong db theo ID
        /// </summary>
        /// <returns>Danh sách đối tượng</returns>
        /// CreatedBy:LNT(27/08)
        public virtual ServiceResult GetById(Guid entityId)
        {
            serviceResult = new ServiceResult();
            try
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add($"${_tableName}Id", entityId.ToString());
                string sqlCommand = $"Proc_Get{_tableName}ById";
                var entity = _dbConnection.QueryFirstOrDefault<TEntity>(sqlCommand, param: parameter, commandType: CommandType.StoredProcedure);
                serviceResult.Data = entity;
                serviceResult.rowEffects = 1;
                return serviceResult;
            }
            catch (Exception ex)
            {
                serviceResult.SetInternalServerError(serviceResult);
                serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception, ex.Message));
                return serviceResult;
            }
        }
        /// <summary>
        /// Lấy thông tin theo giá trị
        /// </summary>
        /// <param name="propertyName">Tên thông tin cần lấy</param>
        /// <param name="propertyValue">Giá trị</param>
        /// <returns>Bản ghi phù hợp</returns>
        /// CreatedBy: LNT (27/08)
        public virtual ServiceResult GetByProperty(string propertyName, string propertyValue)
        {
            serviceResult = new ServiceResult();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                string sqlCommand = $"Proc_Get{_tableName}By{propertyName}";
                parameters.Add("$Value", propertyValue);

                var entity = _dbConnection.QueryFirstOrDefault<TEntity>(sqlCommand, param: parameters, commandType: CommandType.StoredProcedure);
                serviceResult.Data = entity;
                serviceResult.rowEffects = 1;
                return serviceResult;
            }
            catch(Exception ex)
            {
                serviceResult.SetInternalServerError(serviceResult);
                serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception, ex.Message));
                return serviceResult;
            }
            
        }
        /// <summary>
        /// Cập nhật thông tin đối tượng theo ID
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: LNT (27/08)
        public virtual int Update(TEntity entity, Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();

            string sqlCommand = $"Proc_Update{_tableName}";
            foreach (PropertyInfo prop in entity.GetType().GetProperties())
            {
                var value = prop.GetValue(entity) == "" ? null : prop.GetValue(entity);
                parameters.Add($"${prop.Name}", value);
            }
            parameters.Add($"${_tableName}Id", id.ToString());
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                int rowAffects = _dbConnection.Execute(sqlCommand, param: parameters, transaction, commandType: CommandType.StoredProcedure);
                transaction.Commit();
                return rowAffects;
            }
        }

        /// <summary>
        /// Map dữ liệu của 1 entity sang thành dynamic parameters dùng cho truy vấn SQL
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns>dynamic parameters đã được format đúng</returns>
        protected DynamicParameters MappingDbType(TEntity entity)
        {

            try
            {
                var properties = entity.GetType().GetProperties();

                var parameters = new DynamicParameters();

                foreach (var property in properties)
                {
                    var propertyName = property.Name;

                    var notMapPro = property.GetCustomAttributes(typeof(NotMap), true);

                    if (notMapPro.Length == 0)
                    {

                        var propertyValue = property.GetValue(entity);

                        var propertyType = property.PropertyType;

                        if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                        {
                            parameters.Add($"${propertyName}", propertyValue, DbType.String);
                        }
                        else
                        {
                            parameters.Add($"${propertyName}", propertyValue);
                        }
                    }
                }

                return parameters;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }
    
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
