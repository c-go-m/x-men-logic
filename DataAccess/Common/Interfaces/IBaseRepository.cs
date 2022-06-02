using System.Threading.Tasks;

namespace DataAccess.Common.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task InsertAsync(TEntity obj);
        Task<TEntity> GetAsync(string id);
    }
}
