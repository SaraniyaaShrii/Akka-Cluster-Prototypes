using Akka.Actor;
using Akka.Cluster.Common;
using Akka.Cluster.Common.Messages;
using Akka.Configuration.Hocon;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Akka.Cluster.API
{
    public class WebAPIController : ApiController
    {
        public string Get()
        {
            string actorSystemName = ConfigurationManager.AppSettings[Constants.ConfigKeys.ActorSystemName];
            string sectionName = "akka";
            var configuration = ((AkkaConfigurationSection)ConfigurationManager.GetSection(sectionName)).AkkaConfig;
            var actorSystem = ActorSystem.Create(actorSystemName, configuration);

            var requestHandlerActor = actorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "requestHandlerActor");
            Thread.Sleep(5000);

            var routees = requestHandlerActor.Ask<Routees>(new GetRoutees()).Result.Members;

            //while (routees.Any() == false)
            //{
            //    Thread.Sleep(1000);
            //}


            var request = new AttributionRequest()
            {
                FundId = 1,
            };
            requestHandlerActor.Tell(request);

            actorSystem.WhenTerminated.Wait();

            return "welcome";
        }

        //public string Get(string name)
        //{
        //    return "Welcome " + name;
        //}

        //public void Post(string name)
        //{
        //    Get(name);
        //}

        public string Post(AttributionRequest request)
        {
            string actorSystemName = ConfigurationManager.AppSettings[Constants.ConfigKeys.ActorSystemName];
            string sectionName = "akka";
            var configuration = ((AkkaConfigurationSection)ConfigurationManager.GetSection(sectionName)).AkkaConfig;
            var actorSystem = ActorSystem.Create(actorSystemName, configuration);

            var requestHandlerActor = actorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "requestHandlerActor");

            var routees = requestHandlerActor.Ask<Routees>(new GetRoutees()).Result.Members;

            while (routees.Any() == false)
            {
                Thread.Sleep(1000);
            }
           
            requestHandlerActor.Tell(request);

            actorSystem.WhenTerminated.Wait();

            return "Request processing";
        }
    }
}
