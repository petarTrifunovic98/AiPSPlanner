using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.Contracts
{
    public interface IEditRightsAuthorization
    {
        bool HasEditRights(string currentRightHolder);
    }
}
