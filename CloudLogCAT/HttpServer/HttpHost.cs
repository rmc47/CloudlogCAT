using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kayak;
using Kayak.Http;
using Nancy;
using Nancy.Bootstrapper;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace CloudlogCAT.HttpServer
{
    internal sealed class HttpHost
    {
        INancyEngine m_Engine;
        private IScheduler m_Scheduler;
        private IServer m_Server;
        private IPEndPoint m_EndPoint;

        public HttpHost(IPEndPoint endpoint)
        {
            var bootStrapper = NancyBootstrapperLocator.Bootstrapper;
            bootStrapper.Initialise();
            m_Engine = bootStrapper.GetEngine();
            m_EndPoint = endpoint;
        }

        public void Start()
        {
            m_Scheduler = KayakScheduler.Factory.Create(new SchedulerDelegate());
            m_Server = KayakServer.Factory.CreateHttp(new NancyRequestDelegate(m_Engine), m_Scheduler);

            IDisposable disposable = m_Server.Listen(m_EndPoint);
            Thread schedulerWorkerThread = new Thread(SchedulerWorker);
            schedulerWorkerThread.IsBackground = true;
            schedulerWorkerThread.Start(disposable);
            m_Scheduler.Post(GetLocalPort);
        }

        private void GetLocalPort()
        {
            Debug.WriteLine(m_EndPoint.Port);
        }

        private void SchedulerWorker(object disposable)
        {
            using ((IDisposable)disposable)
            {
                m_Scheduler.Start();
            }
        }

        public void Stop()
        {
            m_Scheduler.Stop();
        }

        private class SchedulerDelegate : ISchedulerDelegate
        {
            public void OnException(IScheduler scheduler, Exception e)
            {
                Debug.WriteLine("Error on scheduler.");
                e.DebugStackTrace();
            }

            public void OnStop(IScheduler scheduler)
            {

            }
        }

    }
}
