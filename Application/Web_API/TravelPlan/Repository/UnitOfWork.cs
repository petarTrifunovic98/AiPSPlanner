using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        private ITeamRepository _teamRepository;
        public ITeamRepository TeamRepository
        {
            get
            {
                if (this._teamRepository == null)
                    this._teamRepository = new TeamRepository(_context);
                return this._teamRepository;
            }
        }

        private ITripRepository _tripRepository;
        public ITripRepository TripRepository
        {
            get
            {
                if (this._tripRepository == null)
                    this._tripRepository = new TripRepository(_context);
                return this._tripRepository;
            }
        }

        private IItemRepository _itemRepository;
        public IItemRepository ItemRepository
        {
            get
            {
                if (this._itemRepository == null)
                    this._itemRepository = new ItemRepository(_context);
                return this._itemRepository;
            }
        }

        private ILocationRepository _locationRepository;
        public ILocationRepository LocationRepository
        {
            get
            {
                if (this._locationRepository == null)
                    this._locationRepository = new LocationRepository(_context);
                return this._locationRepository;
            }
        }

        private IAccommodationRepository _accommodationRepository;
        public IAccommodationRepository AccommodationRepository
        {
            get
            {
                if (this._accommodationRepository == null)
                    this._accommodationRepository = new AccommodationRepository(_context);
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

        private IVotableRepository _votableRepository;
        public IVotableRepository VotableRepository
        {
            get
            {
                if (this._votableRepository == null)
                    this._votableRepository = new VotableRepository(_context);
                return this._votableRepository;

            }
        }

        private IRepositoryBase<Vote> _voteRepository;
        public IRepositoryBase<Vote> VoteRepository
        {
            get
            {
                if (this._voteRepository == null)
                    this._voteRepository = new RepositoryBase<Vote>(_context);
                return this._voteRepository;

            }
        }

        private IRepositoryBase<TripType> _tripTypeRepository;
        public IRepositoryBase<TripType> TripTypeRepository
        {
            get
            {
                if (this._tripTypeRepository == null)
                    this._tripTypeRepository = new RepositoryBase<TripType>(_context);
                return this._tripTypeRepository;
            }
        }

        private IAddOnRepository _addOnRepository;
        public IAddOnRepository AddOnRepository
        {
            get
            {
                if (this._addOnRepository == null)
                    this._addOnRepository = new AddOnRepository(_context);
                return this._addOnRepository;
            }
        }

        private IRepositoryBase<Notification> _notificationRepository;
        public IRepositoryBase<Notification> NotificationRepository
        {
            get
            {
                if (this._notificationRepository == null)
                    this._notificationRepository = new RepositoryBase<Notification>(_context);
                return this._notificationRepository;
            }
        }

        public void Dispose()
        {
            if(!_disposed)
                _context.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
