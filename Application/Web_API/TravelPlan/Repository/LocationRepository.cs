using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts.RepositoryContracts;
using TravelPlan.DataAccess;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Repository
{
    public class LocationRepository : RepositoryBase<Location>, ILocationRepository
    {
        public LocationRepository(TravelPlanDbContext context) : base(context)
        { }

        public async Task<Location> GetLocationWithVotable(int LocationId)
        {
            Location location = await _dbSet.Include(location => location.Votable).FirstOrDefaultAsync(location => location.LocationId == LocationId);
            return location;
        }

        public async Task<Location> GetLocationWithAccommodations(int LocationId)
        {
            Location location = await _dbSet.Include(location => location.Votable)
                                            .Include(location => location.Accommodations)
                                            .ThenInclude(accommodation => accommodation.Votable)
                                            .FirstOrDefaultAsync(location => location.LocationId == LocationId);
            return location;
        }
    }
}
