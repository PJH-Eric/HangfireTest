using Dapper;
using Microsoft.Extensions.Configuration;
using ModelDTO.Backstage;
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
    public class DummySyncInfoService : IDummySyncInfoService
    {
        private readonly IGenericRepository genericRepository;
        public DummySyncInfoService(IGenericRepository genericRepository)
        {
            this.genericRepository = genericRepository;
        }

        public async Task<DummySyncInfo> Get(int Id)
        {
            var sql = @"SELECT *
                        FROM dummy_sync_info
                        WHERE id=@id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("id", Id);
            return await genericRepository.Get<DummySyncInfo>(sql, parameters, CommandType.Text);
        }

        public async Task<IEnumerable<DummySyncInfo>> GetDummySyncInfos()
        {
            var sql = @"SELECT *
                        FROM dummy_sync_info
                        WHERE is_enable=1 
                        ORDER BY id";

            return await genericRepository.GetAll<DummySyncInfo>(sql, null, CommandType.Text);
        }

        public async Task<IEnumerable<Host>> GetHostList(List<string> HostIdList)
        {
            var sql = @"SELECT host_id,host_ext_id
                        FROM host_list
                        WHERE host_id IN @host_id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("host_id", HostIdList);
            return await genericRepository.GetAll<Host>(sql, parameters, CommandType.Text);
        }

        public async Task<int> UpdateLastSyncData(int Id, DateTime time, long lastSyncId)
        {
            var sql = @"UPDATE dummy_sync_info 
                        SET last_sync_id=@last_sync_id,
                            last_sync_time=@last_sync_time
                        WHERE id=@id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("last_sync_id", lastSyncId);
            parameters.Add("last_sync_time", time);
            parameters.Add("id", Id);
            return await genericRepository.Execute(sql, parameters, CommandType.Text);
        }

        public async Task<int> UpdateLastSyncData(int Id, DateTime time, int status)
        {
            var sql = @"UPDATE dummy_sync_info 
                        SET last_sync_time=@last_sync_time,
                            status=@status
                        WHERE id=@id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("last_sync_time", time);
            parameters.Add("status", status);
            parameters.Add("id", Id);
            return await genericRepository.Execute(sql, parameters, CommandType.Text);
        }

        public async Task<int> UpdateStatus(int Id, int status)
        {
            var sql = @"UPDATE dummy_sync_info 
                        SET status=@status
                        WHERE id=@id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("status", status);
            parameters.Add("id", Id);
            return await genericRepository.Execute(sql, parameters, CommandType.Text);
        }
    }
}
