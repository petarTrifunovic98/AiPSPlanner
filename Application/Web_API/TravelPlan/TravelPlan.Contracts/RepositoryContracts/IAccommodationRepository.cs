using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Contracts.RepositoryContracts
{
    public interface IAccommodationRepository : IRepositoryBase<Accommodation>
    {
        Task<Accommodation> GetAccommodationWithVotable(int AccommodationId);
        Task<IEnumerable<AccommodationPicture>> GetAccommodationPictures(int accommodationId);
        Task<Accommodation> GetAccommodationWithLocation(int accommodationId);
    }
}
