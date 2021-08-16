using Dapper;
using ModelDTO.Backstage;
using ModelDTO.DmGameLog;
using MySql.Data.MySqlClient;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class GameTableLogService : IGameTableLogService
    {
        private readonly IGenericRepository genericRepository;
        public GameTableLogService(string db, string user, string pwd)
        {
            string ConnectionString = string.Format
                ("server={0};port=3306;database=dmgamelog;user={1};password={2};charset=utf8;Convert Zero Datetime=True;"
                , db, user, pwd);
            this.genericRepository = new GenericRepository
                (new MySqlConnection(ConnectionString));
        }

        public async Task<IEnumerable<GameTableLog>> GetGameTableLogs(long Id, List<string> vHostApiKey, int limit)
        {
            var sql = @"SELECT *
                        FROM game_table_log
                        WHERE id> @Id AND vHostApiKey in @vHostApiKey
                        ORDER BY id limit " + limit;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", Id);
            parameters.Add("vHostApiKey", vHostApiKey);
            return await genericRepository.GetAll<GameTableLog>(sql, parameters, CommandType.Text);
        }
    }
}
