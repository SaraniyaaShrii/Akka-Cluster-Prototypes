using Akka.Cluster.Common.Messages;
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
        static void Main(string[] args)
        {
            Console.WriteLine("Requesting Akka cluster API service for attribution stats...");

            int requestId = 1;

            //while (Console.ReadLine().ToLower() == "y")
            {
                var clusterAPIBaseAddress = ConfigurationManager.AppSettings["ClusterAPIAddress"];
                var requestURI = ConfigurationManager.AppSettings["ClusterAPIPostRequestURI"];

                var request = new AttributionRequest()
                {
                    FundId = 1,
                    RequestId = requestId
                };
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
                Thread.Sleep(1000);
            }
        }
    }
}
