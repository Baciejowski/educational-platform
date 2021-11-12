#region snippet2
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcGreeter;

namespace GrpcWebGreeterClient
{
    class Program
    {
        #region snippet
        static async Task Main(string[] args)
        {

            using var channel = GrpcChannel.ForAddress("http://zpi2021.westeurope.cloudapp.azure.com", new GrpcChannelOptions
            {
                HttpHandler = new GrpcWebHandler(new HttpClientHandler())
            });
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        #endregion
    }
}
#endregion