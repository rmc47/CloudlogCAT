using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;

namespace CloudlogCAT
{
    public sealed class NancyTest : NancyModule
    {
        public NancyTest()
        {
            Get["/"] = _ => "CloudLog CAT is running.";
            Get["/foo/{bar}"] = Bar;
        }

        private string Bar(dynamic parameters)
        {
            return Request.Url.ToString();
        }
    }
}
