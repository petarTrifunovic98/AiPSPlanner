﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class UserAuthenticateResponseDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public string Token { get; set; }
        public int UnseenNotifications { get; set; }
    }
}
