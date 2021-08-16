using ModelDTO.Backstage;
using ModelDTO.SlotGame;
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
    public class AccountingService : IAccountingService
    {
        private readonly IGenericRepository genericRepository;
        public AccountingService(string db, string user, string pwd)
        {
            string ConnectionString = string.Format
                ("Data Source={0};Initial Catalog=slot-game;Persist Security Info=True;User ID={1};Password={2};Connect Timeout=3600;Allow User Variables=True;Convert Zero Datetime=True;"
                , db, user, pwd);
            this.genericRepository = new GenericRepository
                (new MySqlConnection(ConnectionString));
        }
        public async Task<int> InsertAccountingRange(IEnumerable<AccountingBase> accountings)
        {
            var sql = @"INSERT IGNORE INTO accounting
                        (machine_id, host_id, host_level, begin_time, game_time, game_id, subgame_id,
                        member_id, init_cent, bet, denom, total_win, bonus_win, gamble_win, jackpot_win,
                        game_data, game_result, end_cent, bonus, gamble, game_module, game_end, test_account)
                        VALUES
                        (@MachineId, @HostId, @HostLevel, @BeginTime, @GameTime, @GameId, @SubGameId,
                        @MemberId, @InitCent, @Bet, @Denom, @TotalWin, @BonusWin, @GambleWin, @JackpotWin,
                        @GameData, @GameResult, @EndCent, @Bonus, @Gamble, @GameModule, @GameEnd, @TestAccount)";

            return await genericRepository.ExecuteRange(sql, accountings, CommandType.Text);
        }

        public async Task<int> InsertAccountingRobotRange(IEnumerable<AccountingBase> accountingRobots)
        {
            var sql = @"INSERT IGNORE INTO accounting_robot
                        (machine_id, host_id, host_level, begin_time, game_time, game_id, subgame_id,
                        member_id, init_cent, bet, denom, total_win, bonus_win, gamble_win, jackpot_win,
                        game_data, game_result, end_cent, bonus, gamble, game_module, game_end, test_account)
                        VALUES
                        (@MachineId, @HostId, @HostLevel, @BeginTime, @GameTime, @GameId, @SubGameId,
                        @MemberId, @InitCent, @Bet, @Denom, @TotalWin, @BonusWin, @GambleWin, @JackpotWin,
                        @GameData, @GameResult, @EndCent, @Bonus, @Gamble, @GameModule, @GameEnd, @TestAccount)";

            return await genericRepository.ExecuteRange(sql, accountingRobots, CommandType.Text);
        }
    }
}
