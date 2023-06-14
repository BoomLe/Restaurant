using System.Threading.Tasks;

namespace Restaurant.Service.APIReward.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task Start();
        Task Stop();
    }
}
