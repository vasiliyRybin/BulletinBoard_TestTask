using BulletinBoard.DB.Models;
using BulletinBoard.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BulletinBoard.BusinessLayer
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly BulletinsDBContext _context;

        public Repository(BulletinsDBContext dogsDBContext)
        {
            _context = dogsDBContext ?? throw new ArgumentNullException(nameof(dogsDBContext));
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<bool> CreateAsync(T item)
        {
            _context.Set<T>().Add(item);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return true;
            return false;
        }

        public async Task<T> GetFirstOrDefaultAsync(Func<T, bool> predicate)
        {
            return await Task.Run(() => _context.Set<T>().FirstOrDefault(predicate));
        }
    }
}
