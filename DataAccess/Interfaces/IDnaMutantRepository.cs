using DataAccess.Common.Interfaces;
using Entities.Entities;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IDnaMutantRepository : IBaseRepository<DnaMutantEntity>
    {
        public Task<DnaMutantEntity> GetTestByDNAAsync(string DNA);        
        public Task<long> GetMutantCountAsync(bool isMutant);
    }
}
