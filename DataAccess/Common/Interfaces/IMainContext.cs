using MongoDB.Driver;

namespace DataAccess.Common.Interfaces
{
    public interface IMainContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
