using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Enums;
using MISA.CUKCUK.Core.Interfaces.Repository;
using MISA.CUKCUK.Core.ResourcesCore;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MISA.CUKCUK.Infrastructure.Repository
{
    public class InventoryItemRepository : BaseRepository<InventoryItem>, IInventoryItemRepository
    {
        #region Declare
        IConfiguration _configuration;
        public string _connectionString = string.Empty;
        #endregion

        #region Constructor
        public InventoryItemRepository(IConfiguration configuration) : base(configuration)
        {
            _connectionString = _configuration.GetConnectionString("MISACUKCUKConnectionString");
        }
        #endregion

        #region Methods
        /// <summary>
        /// Tìm kiếm và phân trang
        /// </summary>
        /// <param name="pageIndex">STT trang</param>
        /// <param name="pageSize">Số bản ghi cần trả về</param>
        /// <param name="sort">Tiêu chí sắp xếp</param>
        /// <param name="column">Số cột trả về</param>
        /// <param name="condition">Điều kiện tìm kiếm</param>
        /// <returns>Các thông số và danh sách đối tượng được phân trang</returns>
        /// CreatedBy: LNT (03/10)
        public object GetByPaging(int pageIndex, int pageSize, string sort, string column, string condition)
        {
            sort = sort == null ? string.Empty : sort;
            condition = condition == null ? "1 = 1" : condition;
            column = column == null ? "*" : column;
            var param = new DynamicParameters();
            param.Add("$Sort", sort);
            param.Add("$Column", column);
            param.Add("$Condition", condition);
            param.Add("$PageIndex", pageIndex);
            param.Add("$PageSize", pageSize);
            param.Add("$TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("$TotalPage", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var inventoryItems = _dbConnection.Query<InventoryItem>("Proc_FilterAndPaging", param: param, commandType: CommandType.StoredProcedure);
            var totalPage = param.Get<int>("$TotalPage");
            var toatalRecord = param.Get<int>("$TotalRecord");

            var results = new
            {
                StatusCode = (int)EnumStatusCode.Success,
                TotalPage = totalPage,
                TotalRecord = toatalRecord,
                Data = inventoryItems,
            };
            return results;
        }
        /// <summary>
        /// Xóa thông tin theo ID
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// CreatedBy: LNT (04/10)
        public override ServiceResult Delete(Guid entityId)
        {
            MySqlConnection dbConnection = GetConnection();
            dbConnection.Open();
            ServiceResult serviceResult = new ServiceResult();
            var transaction = dbConnection.BeginTransaction();
            try
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("$InventoryItemId", entityId.ToString());
                int rowEffectsInventoryItem = _dbConnection.Execute("Proc_DeleteInventoryItemById", param: parameter, commandType: CommandType.StoredProcedure);

                int rowEffectsInventoryItemUnitConvert = _dbConnection.Execute("Proc_DeleteInventoryItemUnitConvertById", param: parameter, commandType: CommandType.StoredProcedure);
                transaction.Commit();
                serviceResult.Data = rowEffectsInventoryItem;
                serviceResult.rowEffects = rowEffectsInventoryItem;
                return serviceResult;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                serviceResult.SetInternalServerError(serviceResult);
                serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception, ex.Message));
                serviceResult.UserMessage = UserResourceVN.Error_Message;
                return serviceResult;
            }
            finally
            {
                transaction.Dispose();
                dbConnection.Close();
                dbConnection.Dispose();
            }
        }
        public override int Add(InventoryItem entity, List<InventoryItemUnitConvert> list)
        {
            try
            {
                MySqlConnection dbConnection = GetConnection();
                dbConnection.Open();
                var rowEffects = 0;

                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        var parameters = MappingDbType(entity);

                        rowEffects = dbConnection.Execute($"Proc_InsertInventoryItem", param: parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                        for (int i = 0; i < list.Count; i++)
                        {
                            var properties = list[i].GetType().GetProperties();

                            var param = new DynamicParameters();

                            foreach (var property in properties)
                            {
                                var propertyName = property.Name;

                                var notMapPro = property.GetCustomAttributes(typeof(NotMap), true);

                                if (notMapPro.Length == 0)
                                {

                                    var propertyValue = property.GetValue(list[i]);

                                    var propertyType = property.PropertyType;

                                    if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                                    {
                                        param.Add($"${propertyName}", propertyValue, DbType.String);
                                    }
                                    else
                                    {
                                        param.Add($"${propertyName}", propertyValue);
                                    }
                                }
                            }

                            int rowAddEffects = dbConnection.Execute($"Proc_InsertInventoryItemUnitConvert", param: param, commandType: CommandType.StoredProcedure, transaction: transaction);
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        transaction.Dispose();
                        dbConnection.Close();
                        dbConnection.Dispose();
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
        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy: LNT (05/10)
        public override int Add(InventoryItem entity)
        {
            // Thêm mới InventoryItem
            var properties = entity.GetType().GetProperties();
            var InventoryItemId = "InventoryItemId";
            var parameters = new DynamicParameters();
            Guid newInventoryItemId = Guid.NewGuid();
            parameters.Add($"${InventoryItemId}", newInventoryItemId, DbType.String);
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
            Console.WriteLine(parameters);


            // Xử lý List && Thêm mới UnitConverts
            List<DynamicParameters> param = new List<DynamicParameters>();
            if (entity.UnitConverts != null)
                {
                List<InventoryItemUnitConvert> list = entity.UnitConverts;
                
                for (int i = 0; i < list.Count; i++)
                {
                    var InventoryItemUnitConvertId = "InventoryItemUnitConvertId";
                    var paramIndex = new DynamicParameters();
                    Guid newInventoryItemUnitConvertId = Guid.NewGuid();
                    paramIndex.Add($"${InventoryItemUnitConvertId}", newInventoryItemUnitConvertId, DbType.String);
                    foreach (var property in list[i].GetType().GetProperties())
                    {
                        var propertyName = property.Name;

                        var notMapPro = property.GetCustomAttributes(typeof(NotMap), true);

                        if (notMapPro.Length == 0)
                        {

                            var propertyValue = property.GetValue(list[i]);

                            var propertyType = property.PropertyType;

                            if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                            {
                                paramIndex.Add($"${propertyName}", propertyValue, DbType.String);
                            }
                            else
                            {
                                paramIndex.Add($"${propertyName}", propertyValue);
                            }
                        }
                    }
                    paramIndex.Add($"${InventoryItemId}", newInventoryItemId, DbType.String);
                    param.Add(paramIndex);
                    Console.WriteLine(paramIndex);
                }
            }
            MySqlConnection dbConnection = GetConnection();
            dbConnection.Open();
            var rowEffects = 0;
            using (var transaction = dbConnection.BeginTransaction())
            {
                try
                {
                    rowEffects = dbConnection.Execute($"Proc_InsertInventoryItem", param: parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                    for (int i = 0; i < param.Count; i++)
                    {
                        var rowEffectToUnit = dbConnection.Execute($"Proc_InsertInventoryItemUnitConvert", param: param[i], commandType: CommandType.StoredProcedure, transaction: transaction);
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    transaction.Dispose();
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }
            return rowEffects;
        }
        /// <summary>
        /// Cập nhật thông tin bản ghi theo ID
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: LNT (05/10)
        public override int Update(InventoryItem entity, Guid id)
        {
            var properties = entity.GetType().GetProperties();
            var InventoryItemId = "InventoryItemId";
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
            parameters.Add($"${InventoryItemId}", id, DbType.String);
            Console.WriteLine(parameters);

            // Xử lý List && Thêm mới UnitConverts
            List<DynamicParameters> param = new List<DynamicParameters>();
            if (entity.UnitConverts != null)
            {
                List<InventoryItemUnitConvert> list = entity.UnitConverts;

                for (int i = 0; i < list.Count; i++)
                {
                    var InventoryItemUnitConvertId = "InventoryItemUnitConvertId";
                    var paramIndex = new DynamicParameters();
                    Guid newInventoryItemUnitConvertId = Guid.NewGuid();
                    paramIndex.Add($"${InventoryItemUnitConvertId}", newInventoryItemUnitConvertId, DbType.String);
                    foreach (var property in list[i].GetType().GetProperties())
                    {
                        var propertyName = property.Name;

                        var notMapPro = property.GetCustomAttributes(typeof(NotMap), true);

                        if (notMapPro.Length == 0)
                        {

                            var propertyValue = property.GetValue(list[i]);

                            var propertyType = property.PropertyType;

                            if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                            {
                                paramIndex.Add($"${propertyName}", propertyValue, DbType.String);
                            }
                            else
                            {
                                paramIndex.Add($"${propertyName}", propertyValue);
                            }
                        }
                    }
                    paramIndex.Add($"${InventoryItemId}", id, DbType.String);
                    param.Add(paramIndex);
                    Console.WriteLine(paramIndex);
                }
            }

            MySqlConnection dbConnection = GetConnection();
            dbConnection.Open();
            var rowEffects = 0;
            using (var transaction = dbConnection.BeginTransaction())
            {
                try
                {
                    rowEffects = dbConnection.Execute($"Proc_UpdateInventoryItem", param: parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                    var parameterItemId = new DynamicParameters();
                    parameterItemId.Add($"${InventoryItemId}", id, DbType.String);
                    var rowDelete = dbConnection.Execute($"Proc_DeleteInventoryItemUnitConvertById", param: parameterItemId, commandType: CommandType.StoredProcedure, transaction: transaction);
                    if (entity.UnitConverts != null)
                    {
                        for (int i = 0; i < param.Count; i++)
                        {
                            var rowEffectToUnit = dbConnection.Execute($"Proc_InsertInventoryItemUnitConvert", param: param[i], commandType: CommandType.StoredProcedure, transaction: transaction);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    transaction.Dispose();
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }
            return rowEffects;
        }
        /// <summary>
        /// Sinh mã mới
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// CreatedBy: LNT (06/10)
        public string GetNewCode(string code)
        {
            string newCode = string.Empty;
            code = code.ToUpper();
            string[] arrListStr = code.Split(new char[] { ' ' });
            Console.WriteLine(arrListStr[0]);
            Console.WriteLine(arrListStr[0][0]);
            for (int i = 0; i < arrListStr.Length; i++)
            {
                newCode += arrListStr[i][0];
            }
            MySqlConnection dbConnection = GetConnection();
            dbConnection.Open();
            ServiceResult serviceResult = new ServiceResult();
            var transaction = dbConnection.BeginTransaction();
            string codeIndex = string.Empty;
            try
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("$InventoryItemName", newCode);
                codeIndex = _dbConnection.QueryFirstOrDefault<string>("Proc_GetMaxIndex", param: parameter, commandType: CommandType.StoredProcedure);
                transaction.Commit();
                serviceResult.Data = codeIndex;
                //serviceResult.rowEffects = codeIndex;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                serviceResult.SetInternalServerError(serviceResult);
                serviceResult.DevMessage.Add(string.Format(DevResourceVN.Exception, ex.Message));
                serviceResult.UserMessage = UserResourceVN.Error_Message;
                return "Error";
            }
            finally
            {
                transaction.Dispose();
                dbConnection.Close();
                dbConnection.Dispose();
            }
            if (codeIndex != null && Int32.Parse(codeIndex) != 0)
            {
                newCode += (Int32.Parse(codeIndex) + 1);
            }
            else
            {
                newCode += 1;
            }
            return newCode;
        }

        
        #endregion
    }
}
