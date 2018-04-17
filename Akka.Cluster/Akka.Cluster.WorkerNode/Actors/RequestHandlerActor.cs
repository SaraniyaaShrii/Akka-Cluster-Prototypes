using Akka.Actor;
using Akka.Cluster.API.Messages;
using Akka.Common.Messages;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Cluster.WorkerNode.Actors
{
    public class RequestHandlerActor : ReceiveActor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RequestHandlerActor));
        private readonly IActorRef _logWriterActor;

        public RequestHandlerActor(IActorRef logWriterActor)
        {
            _logWriterActor = logWriterActor;

            Receive<AttributionRequest>(requestMsg => Handler(requestMsg));
        }

        private void Handler(AttributionRequest requestMsg)
        {
            unsafe
            {
                try
                {
                    var requestId = requestMsg.RequestId;

                    var currActor_uid = Context.Self.Path.Uid.ToString();
                    var senderActor_uid = Sender.Path.Uid.ToString();
                    var nodeAddr = string.Format("{0}:{1}", Sender.Path.Address.Host, Sender.Path.Address.Port);

                    var self = Context.Self;
                    var parent = Context.Parent;
                    var sys = Context.System;

                    TypedReference typeRef = __makeref(requestMsg);
                    IntPtr ptr = **(IntPtr**)(&typeRef);
                    string msgAddr = ptr.ToString();
                    long msgAddrInt = ptr.ToInt64();

                    string logMsg = string.Format("{0}Some message received. Request_id: {1}, CurrentActor: {2}, Sender: {3}, Obj address: {4}, Node: {5}",
                        Environment.NewLine, requestId, self.ToString(), senderActor_uid, msgAddr, nodeAddr);

                    string filename = string.Format("TestFile-{0}.txt", currActor_uid);
                    string filePath = string.Format("{0}{1}", ConfigurationManager.AppSettings["TestFilePath"], filename);

                    File.AppendAllText(filePath, logMsg);
                    //Logger.Info(logMsg);

                    _logWriterActor.Tell(requestMsg);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

    }
}
