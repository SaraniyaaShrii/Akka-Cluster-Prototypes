using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Cluster.Common.Messages
{
    public class AttributionRequest
    {
        public int FundId { get; set; }

        public DateTime ContextDate { get; set; }

        public List<string> Items { get; set; }

        public List<string> Groups { get; set; }

        public int RequestId { get; set; }
    }

    public class AttributionRequestMessage
    {
        public AttributionRequest Request { get; set; }
    }


}
