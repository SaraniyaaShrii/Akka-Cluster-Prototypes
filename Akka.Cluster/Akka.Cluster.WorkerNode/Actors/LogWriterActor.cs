using System;
using Akka.Actor;
using System.IO;
using Akka.Common.Messages;
using System.Configuration;
using log4net;

namespace Akka.Cluster.API
{
    public class LogWriterActor : ReceiveActor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LogWriterActor));

        public LogWriterActor()
        {
            Receive<AttributionRequest>(msg => Handler(msg));
        }

        private void Handler(AttributionRequest message)
        {
            unsafe
            {
                try
                {
                    var requestId = message.RequestId;

                    var currActor_uid = Context.Self.Path.Uid.ToString();
                    var senderActor_uid = Sender.Path.Uid.ToString();
                    var nodeAddr = string.Format("{0}:{1}", Sender.Path.Address.Host, Sender.Path.Address.Port);

                    var self = Context.Self;
                    var parent = Context.Parent;
                    var sys = Context.System;

                    TypedReference typeRef = __makeref(message);
                    IntPtr ptr = **(IntPtr**)(&typeRef);
                    string msgAddr = ptr.ToString();
                    long msgAddrInt = ptr.ToInt64();

                    string logMsg = string.Format("{0}Some message received. Request_id: {1}, CurrentActor: {2}, Sender: {3}, Obj address: {4}, Node: {5}",
                        Environment.NewLine, requestId, self.ToString(), senderActor_uid, msgAddr, nodeAddr);

                    string filename = string.Format("TestFile-LogActor-{0}.txt", currActor_uid);
                    string filePath = string.Format("{0}{1}", ConfigurationManager.AppSettings["TestFilePath"], filename);

                    File.AppendAllText(filePath, logMsg);

                    //Logger.Info(logMsg);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
    }
}
