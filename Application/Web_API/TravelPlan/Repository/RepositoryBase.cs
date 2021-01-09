using System;
using System.Collections.Generic;
using TravelPlan.DataAccess;
using TravelPlan.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace TravelPlan.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly TravelPlanDbContext _context;
        protected DbSet<T> _dbSet;

        public RepositoryBase(TravelPlanDbContext context)
        {
            _context = context;
            _dbSet = this._context.Set<T>();
        }
        public async Task<IEnumerable<T>> FindAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> FindByID(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Create(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(object id)
        {
            T entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
