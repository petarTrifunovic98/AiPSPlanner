using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        IEnumerable<T> FindAll();
        T FindByID(object id);
        void Create(T entity);
        void Update(T entity);
        void Delete(object id);
    }
}
