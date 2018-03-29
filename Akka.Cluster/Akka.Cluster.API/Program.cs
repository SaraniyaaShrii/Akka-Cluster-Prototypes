using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Akka.Cluster.API
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

                x.UseAssemblyInfoForServiceInfo();
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.Service<WebAPIService>(sc =>
                {
                    sc.ConstructUsing(() => new WebAPIService());
                    sc.WhenStarted(tc => tc.Start());
                    sc.WhenStopped(tc => tc.Stop());
                });

                x.EnableServiceRecovery(r => r.RestartService(1));
            });
        }
    }
}
