using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Nancy;
using RigCAT.NET;

namespace CloudlogCAT
{
    public sealed class RigStatusServer : NancyModule
    {
        private readonly JavaScriptSerializer m_Serializer = new JavaScriptSerializer();

        internal static IRadio Radio { get; set; }

        public RigStatusServer()
        {
            Get["/rigstatus"] = RigStatus;
            Get["/qsy/{vfo}/{frequency}"] = Qsy;
        }

        private Response RigStatus(dynamic parameters)
        {
            if (Radio == null)
                return Response.AsJson(new { error = "No radio connected" }, HttpStatusCode.ServiceUnavailable);

            return Response.AsJson(Radio, HttpStatusCode.OK);
        }

        private Response Qsy(dynamic parameters)
        {
            if (parameters.vfo == null || parameters.frequency == null)
                return Response.AsJson(new { error = "Bad QSY request" }, HttpStatusCode.BadRequest);

            Radio.PrimaryFrequency = long.Parse(parameters.frequency);
            return Response.AsJson(new { status = "OK" }, HttpStatusCode.OK);
        }
    }
}
