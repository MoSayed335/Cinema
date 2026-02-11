using Microsoft.EntityFrameworkCore;

namespace Cinema.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity);

        void ubdate(T entity);

        void Delete(T entity);

        //read operations
        Task<IEnumerable<T>> GetAllasync(Expression<Func<T, bool>>?
           expression = null, Expression<Func<T, object>>[]? includes = null, bool Tracke = true);

        Task<T?> GetoneAsync(Expression<Func<T, bool>>?
            expression = null, Expression<Func<T, object>>[]? includes = null, bool Tracke = true);

        Task<int> CommitAsync();
    }
}
