using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlan.Services.MessagingService
{
    public class MessageControllerService
    {
        private readonly IHubContext<MessageHub> _hubContext;

        public MessageControllerService(IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyOnTripChanges(int tripId, String method, Object object_to_send)
        {
            await _hubContext.Clients.Group("Trip" + tripId).SendAsync(method, object_to_send);
        }
    }
}
