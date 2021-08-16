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
        Task<int> UpdateStatus(int Id , int status);
        Task<int> UpdateLastSyncData(int Id, DateTime time, long lastSyncId);
        Task<int> UpdateLastSyncData(int Id, DateTime time, int status);
        Task<IEnumerable<Host>> GetHostList(List<string> HostIdList);
    }
}
