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
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
