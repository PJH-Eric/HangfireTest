using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireDemo.Domain
{
    public class AppManager
    {
        public static AppSetting AppSetting { get; set; }
    }
    public class AppSetting
    {
        public DbConfig DbConfig { get; set; }
        public Interval Interval { get; set; }
        public string hook_url { get; set; }
        public int limit { get; set; }

    }
    public class Interval
    {
        public int insert_count { get; set; } = 100;
        public int insert_time { get; set; } = 0;
        public int break_time { get; set; } = 30;
    }
    public class DbConfig
    {
        public string User { get; set; }
        public string Password { get; set; }
    }
}
