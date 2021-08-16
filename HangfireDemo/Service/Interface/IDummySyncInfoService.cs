using ModelDTO.Backstage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IDummySyncInfoService
    {
        Task<DummySyncInfo> Get(int Id);
        Task<IEnumerable<DummySyncInfo>> GetDummySyncInfos();
        Task<int> UpdateStatus(int status);
        Task<int> UpdateStatus(int Id , int status);
        Task<int> UpdateLastSyncTime(int Id, DateTime time);
        Task<int> UpdateLastSyncId(int Id, long lastSyncId);
        Task<int> UpdateLastSyncData(int Id, DateTime time,long lastSyncId);
        Task<IEnumerable<Host>> GetHostList(List<string> HostIdList);
    }
}
