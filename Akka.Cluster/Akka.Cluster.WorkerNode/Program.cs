using Akka.Actor;
using Akka.Cluster.API.Actors;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Configuration.Hocon;
using Akka.Common;
using Akka.Common.Util;

namespace Akka.Cluster.API
{
    class Program
    {
        static void Main(string[] args)
        {
            ActorSystem actorSystem = Extensions.CreateActorSystem();

            var consoleWriterActor = actorSystem.ActorOf(Props.Create<LogWriterActor>().WithRouter(FromConfig.Instance), "consoleWriterActor");

            actorSystem.ActorOf(Props.Create(() => new RequestHandlerActor(consoleWriterActor)), "requestHandlerActor");

            actorSystem.WhenTerminated.Wait();
        }
    }
}
