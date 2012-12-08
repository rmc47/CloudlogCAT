using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net;

namespace CloudlogCAT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var nancyHost = new HttpServer.HttpHost(new IPEndPoint(IPAddress.Any, 8083));
            try
            {
                nancyHost.Start();
            }
            catch
            {
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            nancyHost.Stop();
        }
    }
}
