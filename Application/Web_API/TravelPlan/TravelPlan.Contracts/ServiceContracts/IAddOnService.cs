﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Contracts.ServiceContracts
{
    public interface IAddOnService
    {
        Task<List<DecorationAvailableDTO>> GetAvailableDecorations(int tripId);
        Task<AddOnDTO> EditAddOn(AddOnEditDTO addOnInfo, int tripId);
        Task<AddOnDTO> CreateAddOn(AddOnCreateDTO newAddOn);
        Task<bool> DeleteAddOn(int addOnId, int tripId);
        Task<List<AddOnDTO>> GetTripAddOns(int tripId);
    }
}
