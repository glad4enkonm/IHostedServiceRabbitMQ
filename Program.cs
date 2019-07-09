using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Core
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            await new HostBuilder()
                 .ConfigureServices((hostContext, services) =>
                 {
                     services.AddHostedService<ServiceRabbitMQ>(); // register our service here            
                 })
                .RunConsoleAsync();
        }
    }
}
