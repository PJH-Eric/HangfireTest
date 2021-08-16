using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelDTO.DmGameLog
{
    public class GameTableLog
    {
        public long ID { get; set; }
        public string vRoundID { get; set; }
        public string vHashKey { get; set; }
        public string vHostApiKey { get; set; }
        public int vSubGameID { get; set; }
        public DateTime vTime { get; set; }
        public string Log { get; set; }
    }
}
