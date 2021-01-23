using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TravelPlan.Services.MessagingService
{
    public class MessageHub : Hub<IMessageHub>
    {
        public async Task TestHubMethod(string message)
        {
            //TestMethod - metoda IMessageHub interfejsa
            await Clients.All.TestMethod(message);
        }
    }
}
