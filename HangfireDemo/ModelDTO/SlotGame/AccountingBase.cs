using System;
using System.Collections.Generic;
using System.Text;

namespace ModelDTO.SlotGame
{
    public class AccountingBase
    {
		public Int64 Sn { get; set; }
		public int MachineId { get; set; } = -1;
		public string HostId { get; set; }
		public int HostLevel { get; set; } = 0;
		public DateTime BeginTime { get; set; }
		public DateTime GameTime { get; set; }
		public string GameId { get; set; }
		public int SubGameId { get; set; }
		public string MemberId { get; set; }
		public Int64 InitCent { get; set; } = 0;
		public int Bet { get; set; }
		public int Denom { get; set; } = 1;
		public Int64 TotalWin { get; set; }
		public Int64 BonusWin { get; set; } = 0;
		public Int64 GambleWin { get; set; } = 0;
		public Int64 JackpotWin { get; set; } = 0;
		public string GameData { get; set; }
		public string GameResult { get; set; }
		public Int64 EndCent { get; set; }
		public byte Bonus { get; set; } = 0;
		public byte Gamble { get; set; } = 0;
		public string GameModule { get; set; } = "0";
		public byte GameEnd { get; set; } = 1;
		public byte TestAccount { get; set; } = 0;
	}
}
