
using Azure.Messaging.ServiceBus;
using ServiceBus.Interfaces;
using System.Threading.Tasks;

namespace ServiceBus.ServiceBus
{
    public class ServiceBusSend : IServiceBusSend
    {
        public readonly string serviceBusConnection;
        
        public ServiceBusSend(string serviceBusConnection)
        {
            this.serviceBusConnection = serviceBusConnection;
        }
        public async Task<bool> SendToQueueAsync(string queueName, object message)
        {
            if (serviceBusConnection.Trim() != null || serviceBusConnection.Trim() != "")
            {
                //ServiceBus Cliente
                await using (ServiceBusClient client = new ServiceBusClient(serviceBusConnection))
                {
                    //Sender para la cola
                    ServiceBusSender sender = client.CreateSender(queueName);

                    //Mensaje
                    ServiceBusMessage busMessage = new ServiceBusMessage
                    {
                        Body = new System.BinaryData(message)
                    };

                    //Enviar mensaje
                    await sender.SendMessageAsync(busMessage);

                    return true;
                }
            }

            return false;
        }
    }
}
