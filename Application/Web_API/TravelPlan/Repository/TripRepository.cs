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
    public class TripRepository : RepositoryBase<Trip>, ITripRepository
    {
        public TripRepository(TravelPlanDbContext context) : base(context)
        { }

        public async Task<Trip> GetTripWithMembers(int id)
        {
            Trip trip = await _dbSet.Include(trip => trip.Travelers)
                                    .Include(trip => trip.TripType)
                                    .FirstOrDefaultAsync(trip => trip.TripId == id);
            return trip;
        }

        public async Task<Trip> GetTripWithMembersAndLocations(int id)
        {
            Trip trip = await _dbSet.Include(trip => trip.TripType)
                                    .Include(trip => trip.Travelers)
                                    .Include(trip => trip.Locations).ThenInclude(location => location.Votable)
                                    .FirstOrDefaultAsync(trip => trip.TripId == id);
            return trip;
        }

        public async Task<Trip> GetTripWithItemsAndMembers(int id)
        {
            Trip trip = await _dbSet.Include(trip => trip.Travelers)
                                    .Include(trip => trip.ItemList)
                                    .Include(trip => trip.TripType)
                                    .FirstOrDefaultAsync(trip => trip.TripId == id);
            return trip;
        }

        public async Task<Trip> GetTripWithILocations(int id)
        {
            Trip trip = await _dbSet.Include(trip => trip.TripType)
                                    .Include(trip => trip.Locations)
                                    .ThenInclude(x => x.Votable)
                                    .FirstOrDefaultAsync(trip => trip.TripId == id);
            return trip;
        }

        public async Task<IEnumerable<Trip>> GetUserTrips(User user)
        {
            IEnumerable<Trip> trips = await _dbSet.Select(trip => trip).Where(trip => trip.Travelers.Contains(user))
                                                  .Include(trip => trip.Travelers).Include(trip => trip.ItemList)
                                                  .Include(trip => trip.TripType).ToListAsync();
            return trips;
        }

        public async Task<TripType> GetTripTripType(int tripId)
        {
            return await _dbSet.Where(trip => trip.TripId == tripId).Select(trip => trip.TripType).FirstOrDefaultAsync();
        }
    }
}
