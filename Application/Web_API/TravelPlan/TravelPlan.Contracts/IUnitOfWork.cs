using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts.RepositoryContracts;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        ITeamRepository TeamRepository { get; }
        ITripRepository TripRepository { get; }
        IItemRepository ItemRepository { get; }
        ILocationRepository LocationRepository { get; }
        IAccommodationRepository AccommodationRepository { get; }
        IVotableRepository VotableRepository { get; }
        IRepositoryBase<Vote> VoteRepository { get; }
        IRepositoryBase<AccommodationPicture> AccommodationPictureRepository { get; }

        Task<bool> Save();
    }
}
