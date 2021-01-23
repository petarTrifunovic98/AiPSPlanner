using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlan.Contracts
{
    public interface IMessageHub
    {
        Task TestMethod(string testMessage);
    }
}
