using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using RigCAT.NET.CAT;

namespace RigCAT.NET.Elecraft
{
    public sealed class K3 : GenericCATRadio
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
    }
}
