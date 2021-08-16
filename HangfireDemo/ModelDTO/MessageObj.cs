using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace ModelDTO
{
    public class MessageObj
    {
        public string project { get; set; }
        public EventLevel eventLevel { get; set; }
        public string messgae { get; set; }

    }
}
