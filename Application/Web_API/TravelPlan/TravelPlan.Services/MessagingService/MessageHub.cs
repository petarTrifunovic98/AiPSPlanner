using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace TravelPlan.Services.MessagingService
{
    public class MessageHub : Hub
    {
        public async Task NotifyOnTripChanges(int tripId, String method, Object object_to_send)
        {
            await Clients.Group("Trip" + tripId).SendAsync(method, object_to_send);
        }

        public async Task<string> JoinTripGroup(int tripId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Trip" + tripId);
            return "Joined group \"Trip" + tripId + "\"";
        }

        public async Task<string> JoinItemGroup(int userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "User" + userId);
            return "Joined group \"User" + userId + "\"";
        }

        public async Task<string> JoinTeamGroup(int teamId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Team" + teamId);
            return "Joined group \"Team" + teamId + "\"";
        }

        public async Task<string> LeaveTripGroup(int tripId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Trip" + tripId);
            return "Left group \"Trip" + tripId + "\"";
        }

        public async Task<string> LeaveItemGroup(int userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "User" + userId);
            return "Left group \"User" + userId + "\"";
        }

        public async Task<string> LeaveTeamGroup(int teamId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Team" + teamId);
            return "Left group \"Team" + teamId + "\"";
        }
    }
}