using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IGenericRepository : IDisposable
    {
        IDbConnection GetDbconnection();
        Task<T> Get<T>(string sql, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<IEnumerable<T>> GetAll<T>(string sql, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<int> Execute(string sql, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<int> ExecuteRange<T>(string sp, IEnumerable<T> parms, CommandType commandType = CommandType.StoredProcedure);
    }
}
