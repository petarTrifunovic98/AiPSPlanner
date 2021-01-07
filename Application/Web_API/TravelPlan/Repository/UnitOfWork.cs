using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.Contracts;
using TravelPlan.Contracts.RepositoryContracts;
using TravelPlan.DataAccess;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TravelPlanDbContext _context;
        private bool _disposed = false;

        public UnitOfWork(TravelPlanDbContext context)
        {
            _context = context;
        }



        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (this._userRepository == null)
                    this._userRepository = new UserRepository(_context);
                return this._userRepository;
            }
        }

        //-----------------
        private ITeamRepository _teamRepository2;
        public ITeamRepository TeamRepository2
        {
            get
            {
                if (this._teamRepository2 == null)
                    this._teamRepository2 = new TeamRepository(_context);
                return this._teamRepository2;
            }
        }

        //-----------------

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

        private IRepositoryBase<Trip> _tripRepository;
        public IRepositoryBase<Trip> TripRepository
        {
            get
            {
                if (this._tripRepository == null)
                    this._tripRepository = new RepositoryBase<Trip>(_context);
                return this._tripRepository;
            }
        }

        private IRepositoryBase<Location> _locationRepository;
        public IRepositoryBase<Location> LocationRepository
        {
            get
            {
                if (this._locationRepository == null)
                    this._locationRepository = new RepositoryBase<Location>(_context);
                return this._locationRepository;
            }
        }

        private IRepositoryBase<Accommodation> _accommodationRepository;
        public IRepositoryBase<Accommodation> AccommodationRepository
        {
            get
            {
                if (this._accommodationRepository == null)
                    this._accommodationRepository = new RepositoryBase<Accommodation>(_context);
                return this._accommodationRepository;
            }
        }

        private IRepositoryBase<AccommodationPicture> _accommodationPictureRepository;
        public IRepositoryBase<AccommodationPicture> AccommodationPictureRepository
        {
            get
            {
                if (this._accommodationPictureRepository == null)
                    this._accommodationPictureRepository = new RepositoryBase<AccommodationPicture>(_context);
                return this._accommodationPictureRepository;

            }
        }

        public void Dispose()
        {
            if(!_disposed)
                _context.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
