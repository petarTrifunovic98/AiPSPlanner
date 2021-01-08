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
    public class TripRepository : RepositoryBase<Trip>, ITripRepository
    {
        public TripRepository(TravelPlanDbContext context) : base(context)
        { }

        public async Task<Trip> GetTripWithMembers(int id)
        {
            Trip trip = await _dbSet.Include(trip => trip.Travelers).FirstOrDefaultAsync(trip => trip.TripId == id);
            return trip;
        }
    }
}
