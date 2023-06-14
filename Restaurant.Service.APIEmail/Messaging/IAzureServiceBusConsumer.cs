using System.Threading.Tasks;

namespace Restaurant.Service.APIEmail.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task Start();
        Task Stop();
    }
}
