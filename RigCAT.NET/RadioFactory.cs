using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RigCAT.NET
{
    public sealed class RadioFactory
    {
        public IRadio GetRadio(RadioModel model, RadioConnectionSettings connectionSettings)
        {
            switch (model)
            {
                case RadioModel.ElecraftK3:
                    return new Elecraft.K3(connectionSettings);
                case RadioModel.FT950:
                    return new Yaesu.FT950(connectionSettings);
                case RadioModel.IcomGeneric:
                    return new Icom.GenericIcom(connectionSettings);
                default:
                    throw new UnsupportedRadioException();
            }
        }
    }
}
