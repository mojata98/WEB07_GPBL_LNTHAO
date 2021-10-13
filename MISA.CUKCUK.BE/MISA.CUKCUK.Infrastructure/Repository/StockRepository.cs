using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Interfaces.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MISA.CUKCUK.Infrastructure.Repository
{
    public class StockRepository : BaseRepository<Stock>, IStockRepository
    {

        #region Declare
        IConfiguration _configuration;
        public string _connectionString = string.Empty;
        #endregion

        #region Constructor
        public StockRepository(IConfiguration configuration) : base(configuration)
        {
            _connectionString = _configuration.GetConnectionString("MISACUKCUKConnectionString");
        }
        #endregion

        #region Methods
        /// <summary>
        /// Thêm mới dữ liệu vào db
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy: LNT (27/08)
        public override int Add(Stock entity)
        {
            try
            {
                var rowEffects = 0;
                MySqlConnection dbConnection = GetConnection();
                dbConnection.Open();

                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        var parameters = MappingDbType(entity);

                        rowEffects = dbConnection.Execute($"Proc_Insert{_tableName}", param: parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

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

        protected DynamicParameters MappingDbType(Unit entity)
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
        #endregion
    }
}
