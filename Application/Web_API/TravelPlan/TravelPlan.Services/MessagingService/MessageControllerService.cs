using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DTOs.DTOs;

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

        public async Task NotifyOnItemChanges(int userId, String method, Object notification)
        {
            await _hubContext.Clients.Group("User" + userId).SendAsync(method, notification);
        }
    }
}
