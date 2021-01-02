using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.Contracts;
using TravelPlan.DataAccess;

namespace TravelPlan.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TravelPlanDbContext _context;

        public UnitOfWork(TravelPlanDbContext context)
        {
            _context = context;
        }

        /*--------------------------------------------
        Template for creating and exposing repository instances for different entities:
            - if an entity has it's own repository interface (e.g. "IEntityRepository"), then:
                - create an attribute: private IEntityRepository _repository;
                - create a public property:
                public IEntityRepository Repository
                {
                    get
                    {
                        if(this._repository == null)
                            this._repository = new ConcreteEntityRepository(_context);
                        return this._repository;
                    }
                }
            
            - if an entity (e.g. "SomeEntity") doesn't have it's own repository interface, then:
                - create an attribute: private IRepositoryBase<SomeEntity> _repository;
                - create a public property; the difference from the one shown above is that the 
                  type of the property should be IRepositoryBase<SomeEntity>, and it is instantiated
                  with "new RepositoryBase<SomeEntity>(_context)"

        --------------------------------------------*/

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
