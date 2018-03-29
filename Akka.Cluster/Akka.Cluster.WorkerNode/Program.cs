using Akka.Actor;
using Akka.Cluster.Common;
using Akka.Cluster.API.Actors;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Configuration.Hocon;

namespace Akka.Cluster.API
{
    class Program
    {
        static void Main(string[] args)
        {
            string actorSystemName = ConfigurationManager.AppSettings[Constants.ConfigKeys.ActorSystemName];
            string sectionName = "akka";
            var configuration = ((AkkaConfigurationSection)ConfigurationManager.GetSection(sectionName)).AkkaConfig;
            var actorSystem = ActorSystem.Create(actorSystemName, configuration);


            var consoleWriterActor = actorSystem.ActorOf(Props.Create<ConsoleWriterActor>().WithRouter(FromConfig.Instance), "consoleWriterActor");

            actorSystem.ActorOf(Props.Create(() => new RequestHandlerActor(consoleWriterActor)), "requestHandlerActor");

            actorSystem.WhenTerminated.Wait();
        }
    }
}
