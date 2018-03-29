using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Akka.Cluster.API
{
    public class WebAPIService
    {
        private readonly HttpSelfHostServer selfHostServer;
        private string EventSource;

        public WebAPIService()
        {
            var baseAddress = ConfigurationManager.AppSettings["ServiceSelfHostAddress"];
            var selfHostConfig = new HttpSelfHostConfiguration(baseAddress);
            selfHostConfig.Routes.MapHttpRoute
            (
                name: "AkkaClusterAPI",
                //routeTemplate: "api/{controller}/{id}",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            CreateEventLog(baseAddress);

            selfHostServer = new HttpSelfHostServer(selfHostConfig);
        }

        public void Start()
        {
            EventLog.WriteEntry(EventSource, "Starting HttpApiService server.");
            selfHostServer.OpenAsync();
        }

        public void Stop()
        {
            EventLog.WriteEntry(EventSource, "Stopping HttpApiService server.");
            selfHostServer.CloseAsync().Wait();
            selfHostServer.Dispose();
        }

        private void CreateEventLog(string baseAddress)
        {
            EventSource = ConfigurationManager.AppSettings["ServiceName"].ToString();
            if (!EventLog.SourceExists(EventSource))
            {
                EventLog.CreateEventSource(EventSource, "Application");
            }
            EventLog.WriteEntry(EventSource, String.Format("Creating server at {0}", baseAddress.ToString()));
        }
    }
}
