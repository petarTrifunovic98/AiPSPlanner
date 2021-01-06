using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.Contracts;
using TravelPlan.DataAccess;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TravelPlanDbContext _context;

        public UnitOfWork(TravelPlanDbContext context)
        {
            _context = context;
        }


        private IRepositoryBase<User> _userRepository;
        public IRepositoryBase<User> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                    this._userRepository = new RepositoryBase<User>(_context);
                return this._userRepository;
            }
        }

        private IRepositoryBase<Team> _teamRepository;
        public IRepositoryBase<Team> TeamRepository
        {
            get
            {
                if (this._teamRepository == null)
                    this._teamRepository = new RepositoryBase<Team>(_context);
                return this._teamRepository;
            }
        }

        private IRepositoryBase<Item> _itemRepository;
        public IRepositoryBase<Item> ItemRepository
        {
            get
            {
                if (this._itemRepository == null)
                    this._itemRepository = new RepositoryBase<Item>(_context);
                return this._itemRepository;
            }
        }

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
