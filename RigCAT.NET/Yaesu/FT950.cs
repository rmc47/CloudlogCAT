using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RigCAT.NET.CAT;

namespace RigCAT.NET.Yaesu
{
    internal sealed class FT950 : GenericCATRadio
    {
        public FT950(RadioConnectionSettings rcs)
            : base(rcs)
        {
        }

        protected override string RadioModelName
        {
            get { return "Yaesu FT-950"; }
        }

        protected override string GetModeCommand
        {
            get { return "MD0;"; }
        }
    }
}
