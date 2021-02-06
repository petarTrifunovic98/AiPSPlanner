using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.Contracts;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Services.AuthentificationService
{
    public class EditRightsAuthorization: IEditRightsAuthorization
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public EditRightsAuthorization(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public bool HasEditRights(string currentRightHolder)
        {
            User user = (User)_contextAccessor.HttpContext.Items["User"];
            string tokenId = user.UserId.ToString();
            return tokenId == currentRightHolder;
        }
    }
}
