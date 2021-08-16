using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelDTO.Backstage
{
    public class DummySyncInfo
    {
        public int Id { get; set; }
        public string Fromdb { get; set; }
        public string Todb { get; set; }
        public string GameId { get; set; }
        public string HostListString { set; get; }
        public long LastSyncId { get; set; }
        public DateTime LastSyncTime { get; set; }
        public bool IsEnable { get; set; }
        public int Status { get; set; }
        public List<string> HostList
        { 
            get 
            {
                return JsonConvert.DeserializeObject<List<string>>(HostListString);
            } 
        }
        public string FromDbUser { get; set; }
        public string FromDbPwd { get; set; }
        public string ToDbUser { get; set; }
        public string ToDbPwd { get; set; }
    }
}
