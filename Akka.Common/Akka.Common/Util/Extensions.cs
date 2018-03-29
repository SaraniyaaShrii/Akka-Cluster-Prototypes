using Akka.Actor;
using Akka.Configuration.Hocon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Common.Util
{
    public static class Extensions
    {
        public static Akka.Actor.ActorSystem CreateActorSystem()
        {
            string sectionName = "akka";
            var configuration = ((AkkaConfigurationSection)ConfigurationManager.GetSection(sectionName)).AkkaConfig;
            string actorSystemName = ConfigurationManager.AppSettings[Constants.ConfigKeys.ActorSystemName];
            var actorSystem = Akka.Actor.ActorSystem.Create(actorSystemName, configuration);
            return actorSystem;
        }
    }

}
