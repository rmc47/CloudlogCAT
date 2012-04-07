using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RigCAT.NET
{
    public enum OperatingMode
    {
        Unknown = 0,
        LSB = 1,
        USB = 2,
        CW = 3,
        FM = 4,
        AM = 5,
        Data = 6,
        CWRev = 7,
        DataRev = 8
    }
}
