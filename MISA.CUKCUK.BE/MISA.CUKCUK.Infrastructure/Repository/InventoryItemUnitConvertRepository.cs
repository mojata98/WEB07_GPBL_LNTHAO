using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Interfaces.Repository;
using MISA.CUKCUK.Core.ResourcesCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MISA.CUKCUK.Infrastructure.Repository
{
    public class InventoryItemUnitConvertRepository : BaseRepository<InventoryItemUnitConvert>, IInventoryItemUnitConvertRepository
    {
        #region Declare
        IConfiguration _configuration;
        public string _connectionString = string.Empty;
        #endregion

        #region Constructor
        public InventoryItemUnitConvertRepository(IConfiguration configuration) : base(configuration)
        {
            _connectionString = _configuration.GetConnectionString("MISACUKCUKConnectionString");
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy thông tin thei ID
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// CreatedBy: LNT (27/08)
        public override ServiceResult GetById(Guid entityId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add($"$InventoryItemId", entityId.ToString());
                string sqlCommand = $"Proc_GetInventoryItemUnitConvertByInventoryItemId";
                var entity = _dbConnection.Query<InventoryItemUnitConvert>(sqlCommand, param: parameter, commandType: CommandType.StoredProcedure);
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
        #endregion
    }
}
