using ModelDTO.SlotGame;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IAccountingService
    {
        Task<int> InsertAccountingRange(IEnumerable<AccountingBase> accountings);
        Task<int> InsertAccountingRobotRange(IEnumerable<AccountingBase> accountingRobots);
    }
}
