using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Cluster.API.Actors
{
    public static class AkkaComponents
    {
        public static ActorSystem ActorSystem;

        public static IActorRef RequestHandlerActor = ActorRefs.Nobody;

    }
}
