using ModelDTO.DmGameLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IGameTableLogService
    {
        Task<IEnumerable<GameTableLog>> GetGameTableLogs(long Id, List<string> vHostApiKey, int limit);
    }
}
