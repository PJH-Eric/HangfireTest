using Dapper.FluentMap.Mapping;
using ModelDTO.Backstage;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelDTO.FluentMapper
{
    public class DummySyncInfoMap : EntityMap<DummySyncInfo>
    {
        public DummySyncInfoMap()
        {
            Map(x => x.Id).ToColumn("id", false);
            Map(x => x.Fromdb).ToColumn("fromdb", false);
            Map(x => x.Todb).ToColumn("todb", false);
            Map(x => x.GameId).ToColumn("game_id", false);
            Map(x => x.HostListString).ToColumn("host_list", false);
            Map(x => x.LastSyncId).ToColumn("last_sync_id", false);
            Map(x => x.LastSyncTime).ToColumn("last_sync_time", false);
            Map(x => x.IsEnable).ToColumn("is_enable", false);
            Map(x => x.Status).ToColumn("status", false);
            Map(x => x.FromDbUser).ToColumn("fromdbuser", false);
            Map(x => x.FromDbPwd).ToColumn("fromdbpwd", false);
            Map(x => x.ToDbUser).ToColumn("todbuser", false);
            Map(x => x.ToDbPwd).ToColumn("todbpwd", false);
            Map(x => x.HostList).Ignore();
        }
    }
    public class HostMap : EntityMap<Host>
    {
        public HostMap()
        {
            Map(x => x.HostId).ToColumn("host_id", false);
            Map(x => x.HostExtId).ToColumn("host_ext_id", false);
        }
    }
}
