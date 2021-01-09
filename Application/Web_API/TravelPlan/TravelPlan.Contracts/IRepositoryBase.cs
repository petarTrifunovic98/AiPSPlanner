using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlan.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IEnumerable<T>> FindAll();
        Task<T> FindByID(object id);
        Task Create(T entity);
        void Update(T entity);
        void Delete(object id);
        void Delete(T entity);
    }
}
