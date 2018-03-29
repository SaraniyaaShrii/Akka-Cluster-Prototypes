using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Akka.Cluster.Lighthouse
{
    class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                var serviceName = ConfigurationManager.AppSettings["ServiceName"];
                var serviceDisplayName = ConfigurationManager.AppSettings["ServiceDisplayName"];
                var serviceDesc = ConfigurationManager.AppSettings["ServiceDescription"];

                x.SetServiceName(serviceName);
                x.SetDisplayName(serviceDisplayName);
                x.SetDescription(serviceDesc);

                x.Service<LighthouseService>(sc =>
                {
                    sc.ConstructUsing(() => new LighthouseService());

                    // the start and stop methods for the service
                    sc.WhenStarted(s => s.Start());
                    sc.WhenStopped(s => s.StopAsync().Wait());
                });

                x.EnableServiceRecovery(r => r.RestartService(1));
            });
        }
    }
}
