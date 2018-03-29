using Akka.Actor;
using Akka.Cluster.API.Messages;
using Akka.Cluster.Common.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Cluster.API.Actors
{
    public class RequestHandlerActor : ReceiveActor
    {
        private readonly IActorRef _consoleWriterActor;

        public RequestHandlerActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;

            ReceiveAny(requestMsg =>
            {
                unsafe
                {
                    TypedReference tr = __makeref(requestMsg);
                    IntPtr ptr = **(IntPtr**)(&tr);

                    var request = (AttributionRequest)requestMsg;

                    string consoleMsg = string.Format("{0}Some message received. Request_id: {1}, Sender: {2}, Obj address: {3}", 
                        Environment.NewLine, request.RequestId, Sender.Path.Uid.ToString(), ptr.ToString());
                    string filePath = @"B:\Prototypes\Akka-samples 3\Test files\TestFile.txt";
                    File.AppendAllText(filePath, consoleMsg);

                    _consoleWriterActor.Tell(requestMsg);

                }

            });
        }
    }
}
