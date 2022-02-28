using System;
using System.Linq;
using System.Threading.Tasks;

namespace BulletinBoard.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        IQueryable<T> Get();
        Task<T> GetFirstOrDefaultAsync(Func<T, bool> predicate);
        Task<bool> CreateAsync(T item);
    }
}
