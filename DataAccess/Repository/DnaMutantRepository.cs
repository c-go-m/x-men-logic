using DataAccess.Common;
using DataAccess.Common.Interfaces;
using DataAccess.Interfaces;
using Entities.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class DnaMutantRepository : BaseRepository<DnaMutantEntity>, IDnaMutantRepository
    {
        public DnaMutantRepository(IMainContext context) : base(context)
        {
            
        }

        public async Task<long> GetMutantCountAsync(bool isMutant)
        {
            FilterDefinition<DnaMutantEntity> filter = Builders<DnaMutantEntity>.Filter.Eq("IsMutant", isMutant);
            var result = await Collection.CountDocumentsAsync(filter);

            return result;
        }

        public async Task<DnaMutantEntity> GetTestByDNAAsync(string DNA)
        {            
            FilterDefinition<DnaMutantEntity> filter = Builders<DnaMutantEntity>.Filter.Eq("Data", DNA);
            var result = await Collection.FindAsync(filter);

            return result.FirstOrDefault();
        }
    }
}
