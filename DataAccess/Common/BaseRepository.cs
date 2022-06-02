
using DataAccess.Common.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Threading.Tasks;

namespace DataAccess.Common
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        public IMongoCollection<TEntity> Collection { get; private set; }
        public IMainContext Context { get; private set; }
        public BaseRepository(IMainContext context)
        {
            this.Context = context;
            var collectionName = typeof(TEntity).GetCustomAttribute<TableAttribute>(false).Name;
            Collection = context.GetCollection<TEntity>(collectionName);
        }
        public async Task<TEntity> GetAsync(string id)
        {
            var objectId = new ObjectId(id);

            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            var result = await Collection.FindAsync(filter);

            return result.FirstOrDefault();
        }

        public async Task InsertAsync(TEntity obj)
        {
            await Collection.InsertOneAsync(obj);
        }
    }
}
