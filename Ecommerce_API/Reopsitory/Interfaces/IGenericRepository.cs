using Ecommerce_API.Models;

namespace Ecommerce_API.Reopsitory.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task DeleteAsyncByEntity(T entity);
        Task SaveChangesAsync();

    }
}



