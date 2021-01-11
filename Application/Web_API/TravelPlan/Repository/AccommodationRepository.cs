using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts.RepositoryContracts;
using TravelPlan.DataAccess;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Repository
{
    public class AccommodationRepository : RepositoryBase<Accommodation>, IAccommodationRepository
    {
        public AccommodationRepository(TravelPlanDbContext context) : base(context)
        { }

        public async Task<Accommodation> GetAccommodationWithVotable(int AccommodationId)
        {
            Accommodation accommodation = await _dbSet.Include(accommodation => accommodation.Votable).FirstOrDefaultAsync(accommodation => accommodation.AccommodationId == AccommodationId);
            return accommodation;
        }

        public async Task<IEnumerable<AccommodationPicture>> GetAccommodationPictures(int accommodationId)
        {
            IEnumerable<AccommodationPicture> pictures = await _dbSet.Where(accommodation => accommodation.AccommodationId == accommodationId)
                                                                     .Select(accommodation => accommodation.Pictures).FirstOrDefaultAsync();
            return pictures;
        }
    }
}
