using Entities.DTO;
using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IDnaMutant
    {
        public Task<ResponseStats> StatsAsync();

        public Task CreateDnaMutant(DnaMutantEntity dnaMutant);

        Task<bool> ValidDNA(List<string> dna);
    }
}
