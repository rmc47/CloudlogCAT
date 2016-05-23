using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RigCAT.NET
{
    public interface IRadio
    {
        event EventHandler<EventArgs> FrequencyChanged;

        long PrimaryFrequency { get; set; }
        long SecondaryFrequency { get; set; }
        OperatingMode PrimaryMode { get; set; }
        void EqualiseVFOs();
    }
}
