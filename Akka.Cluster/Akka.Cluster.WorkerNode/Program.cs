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
using log4net;

namespace Akka.Cluster.API
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Akka.Cluster.WorkerNode";
            log4net.Config.XmlConfigurator.Configure();
            ActorSystem actorSystem = Extensions.CreateActorSystem();

            var logWriterActor = actorSystem.ActorOf(Props.Create<LogWriterActor>().WithRouter(FromConfig.Instance), "logWriterActor");

            //var reqHandlerProps = Props.Create(() => new RequestHandlerActor(logWriterActor)).WithRouter(FromConfig.Instance);
            //var requestHandlerActor = actorSystem.ActorOf(reqHandlerProps, "requestHandlerActor");

            var requestHandlerActor = actorSystem.ActorOf(Props.Create(() => new RequestHandlerActor(logWriterActor)), "requestHandlerActor");

            var logWriterRoutees = logWriterActor.Ask<Routees>(new GetRoutees()).Result.Members;
            var reqHandlerRoutees = requestHandlerActor.Ask<Routees>(new GetRoutees()).Result.Members;

            actorSystem.WhenTerminated.Wait();
        }
    }
}

