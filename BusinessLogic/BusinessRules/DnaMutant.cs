using BusinessLogic.Interfaces;
using DataAccess.Interfaces;
using Entities.DTO;
using Entities.Entities;
using ServiceBus.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.BusinessRules
{
    public partial class DnaMutant : IDnaMutant
    {
        private int amountString;
        private List<string> localDna;


        private readonly IDnaMutantRepository dataAccessDnaMutant;
        private readonly IServiceBusSend serviceBusSend;
        public DnaMutant(IDnaMutantRepository dataAccessDnaMutant, IServiceBusSend serviceBusSend)
        {
            this.dataAccessDnaMutant = dataAccessDnaMutant;
            this.serviceBusSend = serviceBusSend;
        }
        public async Task CreateDnaMutant(DnaMutantEntity dnaMutant)
        {
            bool exist = await ValidateTestDuplicate(dnaMutant);
            if (!exist)
            {
                await RegistryTest(dnaMutant);
            }
        }

        public async Task<ResponseStats> StatsAsync()
        {
            long countMutant = await dataAccessDnaMutant.GetMutantCountAsync(true);
            long countHuman = await dataAccessDnaMutant.GetMutantCountAsync(false);

            ResponseStats stast = new ResponseStats
            {
                Count_mutant_dna = countMutant,
                Count_human_dna = countHuman,
                Ratio = GetRatio(countMutant, countHuman)
            };

            return stast;
        }

        public async Task<bool> ValidDNA(List<string> dna)
        {
            localDna = dna;
            ValidData();

            bool result = ValidMutant();
            await SendMessaggeServiceBus(result);
            return result;
        }
    }
}
