using System;
using System.Collections.Generic;
using TravelPlan.DataAccess;
using TravelPlan.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TravelPlan.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly TravelPlanDbContext _context;
        private DbSet<T> _dbSet;

        public RepositoryBase(TravelPlanDbContext context)
        {
            _context = context;
            _dbSet = this._context.Set<T>();
        }
        public IEnumerable<T> FindAll()
        {
            return _dbSet.ToList();
        }

        public T FindByID(object id)
        {
            return _dbSet.Find(id);
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
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
    }
}
