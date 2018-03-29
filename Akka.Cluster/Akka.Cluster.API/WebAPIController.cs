using Akka.Actor;
using Akka.Cluster.API.Actors;
using Akka.Common.Messages;
using Akka.Common.Util;
using Akka.Configuration.Hocon;
using Akka.Routing;
using Newtonsoft.Json.Linq;
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
            var request = new AttributionRequest()
            {
                FundId = 1,
            };

            InitiateProcess(request);

            return "welcome";
        }
        
        [HttpPost]
        public string Post([FromBody]JObject request)
        {
            var attribReq = request.ToObject<AttributionRequest>();

            InitiateProcess(attribReq);

            //actorSystem.WhenTerminated.Wait();

            return "";
        }

        private void InitiateProcess(AttributionRequest request)
        {
            //ActorSystem actorSystem = Akka.Common.Util.Extensions.CreateActorSystem();

            //var requestHandlerActor = actorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "requestHandlerActor");

            //var requestHandlerActor = AkkaComponents.ActorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "requestHandlerActor");
            var requestHandlerActor = AkkaComponents.RequestHandlerActor;

            IEnumerable<Routee> routees;
            do
            {
                Thread.Sleep(1000);
                routees = requestHandlerActor.Ask<Routees>(new GetRoutees()).Result.Members;
            } while (routees.Any() == false);

            requestHandlerActor.Tell(request);
        }
    }
}
