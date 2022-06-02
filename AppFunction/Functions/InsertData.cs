using Azure.Messaging.ServiceBus;
using BusinessLogic.Interfaces;
using Common.Constants;
using Entities.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppFunction.Functions
{
    public class InsertData
    {
        private readonly IDnaMutant dnaMutant;
        public InsertData(IDnaMutant dnaMutant)
        {
            this.dnaMutant = dnaMutant;
        }

        [FunctionName("InsertData")]
        public async Task Run([ServiceBusTrigger(Constants.QueueName,
            Connection = "ServiceBusConnection")] ServiceBusReceivedMessage messages,ServiceBusMessageActions messageActions)
        {
            try
            {
                var dna = JsonSerializer.Deserialize<DnaMutantEntity>(messages.Body);
                await dnaMutant.CreateDnaMutant(dna);
                await messageActions.CompleteMessageAsync(messages);
            }
            catch (Exception)
            {
                await messageActions.DeadLetterMessageAsync(messages);                
            }
        }
    }
}
