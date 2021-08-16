using System;
using System.Collections.Generic;
using System.Text;
using Dapper.FluentMap;

namespace ModelDTO.FluentMapper
{
    public static class FluentMapperInit
    {
        public static void Init()
        {
            Dapper.FluentMap.FluentMapper.Initialize(cfg =>
            {
                cfg.AddMap(new DummySyncInfoMap());
                cfg.AddMap(new HostMap());
            });
        }
    }
}
