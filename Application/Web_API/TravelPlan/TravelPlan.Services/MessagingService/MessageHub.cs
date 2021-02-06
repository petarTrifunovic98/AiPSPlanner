using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TravelPlan.Services.MessagingService
{
    public class MessageHub : Hub
    {
        public async Task NotifyOnTripChanges(int tripId, String method, Object object_to_send)
        {
            await Clients.Group("Trip" + tripId).SendAsync(method, object_to_send);
        }
    }
}
