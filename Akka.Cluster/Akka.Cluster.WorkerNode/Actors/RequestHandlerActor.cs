using Akka.Actor;
using Akka.Cluster.API.Messages;
using Akka.Common.Messages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Cluster.API.Actors
{
    public class RequestHandlerActor : ReceiveActor
    {
        private readonly IActorRef _logWriterActor;

        public RequestHandlerActor(IActorRef logWriterActor)
        {
            _logWriterActor = logWriterActor;

            ReceiveAny(requestMsg =>
            {
                unsafe
                {
                    TypedReference tr = __makeref(requestMsg);
                    IntPtr ptr = **(IntPtr**)(&tr);

                    var request = (AttributionRequest)requestMsg;

                    string logMsg = string.Format("{0}Some message received. Request_id: {1}, Sender: {2}, Obj address: {3}", 
                        Environment.NewLine, request.RequestId, Sender.Path.Uid.ToString(), ptr.ToString());

                    string filename = "TestFile.txt";
                    string filePath = string.Format("{0}{1}", ConfigurationManager.AppSettings["TestFilePath"], filename);

                    File.AppendAllText(filePath, logMsg);

                    _logWriterActor.Tell(requestMsg);

                }

            });
        }
    }
}
