using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RigCAT.NET
{
    public class RadioConnectionSettings
    {
        public string Port { get; set; }
        public bool UseDTR { get; set; }
        public bool UseRTS { get; set; }
        public int BaudRate { get; set; }
        public FlowControl FlowControl { get; set; }
    }
}
