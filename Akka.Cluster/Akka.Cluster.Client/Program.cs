using Akka.Common.Messages;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Akka.Cluster.Client
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            Console.Title = "Akka.Cluster.Client";

            Thread.Sleep(5000);

            log4net.Config.XmlConfigurator.Configure();
            Logger.InfoFormat("Requesting Akka cluster API service for attribution stats...");
            Console.WriteLine("Requesting Akka cluster API service for attribution stats...");

            int requestId = 1;
            int randomRequestCount = 6;

            object lockObject = new object();

            Task[] tasks = new Task[randomRequestCount];

            for (int index = 0; index < tasks.Count(); index++)
            {
                int id = requestId;
                tasks[index] = Task.Factory.StartNew(() => RequestAttributionAPI(id));
                lock (lockObject)
                {
                    requestId++;
                }
            }

            Task.WaitAll(tasks);

            Console.WriteLine("Finished....");
            Logger.InfoFormat("Finished....");
        }

        private static void RequestAttributionAPI(int requestId)
        {
            var clusterAPIBaseAddress = ConfigurationManager.AppSettings["ClusterAPIAddress"];
            var requestURI = ConfigurationManager.AppSettings["ClusterAPIPostRequestURI"];

            AttributionRequest request = GetRequest(requestId);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(clusterAPIBaseAddress);
            //HttpResponseMessage response = client.GetAsync(requestURI).Result;
            HttpResponseMessage response = client.PostAsJsonAsync(requestURI, content).Result;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Request processed.");
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            Thread.Sleep(100);
        }

        private static AttributionRequest GetRequest(int requestId)
        {
            return new AttributionRequest()
            {
                FundId = 1,
                RequestId = requestId
            };
        }
    }
}
