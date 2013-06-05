using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace CloudlogCAT.API
{
    internal sealed class CATModel
    {
        public string Radio { get; set; }
        public long Frequency { get; set; }
        public string Mode { get; set; }
        public DateTime Timestamp { get; set; }

        public sealed class Converter : JavaScriptConverter
        {
            public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
            {
                CATModel model = (CATModel)obj;
                Dictionary<string, object> d = new Dictionary<string, object>();
                d["radio"] = model.Radio;
                d["frequency"] = model.Frequency;
                d["mode"] = model.Mode;
                d["timestamp"] = model.Timestamp;
                return d;
            }

            public override IEnumerable<Type> SupportedTypes
            {
                get { return new List<Type> { typeof(CATModel) }; }
            }
        }

    }
}
