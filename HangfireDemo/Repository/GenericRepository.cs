using Dapper;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class GenericRepository : IGenericRepository
    {
        private readonly IDbConnection _db;
        public GenericRepository(IDbConnection db)
        {
            _db = db;
        }
        public void Dispose()
        {
            _db?.Dispose();
        }
        public async Task<int> ExecuteRange<T>(string sp, IEnumerable<T> parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var connection = _db) 
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                }
                catch (Exception)
                {
                    throw;
                }
                return await connection.ExecuteAsync(sp, parms, commandType: commandType);
            }
        }
        public async Task<int> Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var connection = _db)
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                }
                catch (Exception)
                {
                    throw;
                }
                return await connection.ExecuteAsync(sp, parms, commandType: commandType);
            }
        }

        public async Task<T> Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            using (var connection = _db)
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                }
                catch (Exception)
                {
                    throw;
                }
                return await connection.QueryFirstAsync<T>(sp, parms, commandType: commandType);
            }
        }

        public async Task<IEnumerable<T>> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var connection = _db)
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                }
                catch (Exception)
                {
                    throw;
                }
                return await connection.QueryAsync<T>(sp, parms, commandType: commandType);
            }
        }

        public IDbConnection GetDbconnection()
        {
            return _db;
        }
    }
}
