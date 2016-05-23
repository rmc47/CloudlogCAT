using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RigCAT.NET
{
    public interface IWinKey
    {
        void SendString(string str);
        void StopSending();
    }
}
