using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RigCAT.NET
{
    public interface IVoiceKeyer
    {
        void SendDvk(int messageNumber);
        void CancelDvk();
    }
}
