using System.Threading.Tasks;

namespace ServiceBus.Interfaces
{
    public interface IServiceBusSend
    {
        public Task<bool> SendToQueueAsync(string queueName, object message);
    }
}
