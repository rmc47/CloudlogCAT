using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using RigCAT.NET.CAT;

namespace RigCAT.NET.Elecraft
{
    public sealed class K3 : GenericCATRadio, IWinKey, IVoiceKeyer
    {
        public K3(RadioConnectionSettings rcs)
            : base(rcs)
        {
        }

        protected override string RadioModelName
        {
            get { return "Elecraft K3"; }
        }

        protected override string GetModeCommand
        {
            get { return "MD;"; }
        }

        public bool SpeakersAndPhones
        {
            get
            {
                SendQuery("MN097;", false);
                SendQuery("UP;", false);
                SendQuery("MN255;", false);
                return false;
            }
            set
            {

            }
        }

        public void SendString(string str)
        {
            SendQuery("KY " + str + ";", false);
        }

        public void StopSending()
        {
            SendQuery("RX;", false);
        }

        public void SendDvk(int number)
        {
            int switchNumber;
            switch (number)
            {
                case 1: switchNumber = 21; break;
                case 2: switchNumber = 31; break;
                case 3: switchNumber = 35; break;
                case 4: switchNumber = 39; break;
                default: throw new ArgumentOutOfRangeException("number", "DVK number must be between 1 and 4; was " + number); break;
            }

            SendQuery("SWT" + switchNumber + ";", false);
        }

        public void CancelDvk()
        {
            StopSending();
        }
    }
}
