using System;
using Akka.Actor;
using System.IO;
using Akka.Cluster.Common.Messages;

namespace Akka.Cluster.API
{
    public class ConsoleWriterActor : ReceiveActor
    {
        public ConsoleWriterActor()
        {
            ReceiveAny(msg => Handler(msg));
        }
        protected void Handler(object message)
        {
            unsafe
            {
                TypedReference tr = __makeref(message);
                IntPtr ptr = **(IntPtr**)(&tr);

                var request = (AttributionRequest)message;
                var actor_uid = Sender.Path.Uid.ToString();
                string consoleMsg = string.Format("{0}Some message received. Request_id: {1}, Sender: {2}, Obj address: {3}", 
                    Environment.NewLine, request.RequestId, actor_uid, ptr.ToString());
                string filePath = @"B:\Prototypes\Akka-samples 3\Test files\TestFile-222-" + actor_uid + ".txt";
                //File.AppendAllText(filePath, consoleMsg);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(consoleMsg);
                Console.ResetColor();

            }

        }
    }
}
