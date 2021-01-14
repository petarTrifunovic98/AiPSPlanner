using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Contracts.ServiceContracts
{
    public interface IAddOnService
    {
        Task<List<DecorationAvailableDTO>> GetAvailableDecorations(int tripId);
        Task<AddOnDTO> EditAddOn(AddOnEditDTO addOnInfo);
        Task<AddOnDTO> CreateAddOn(AddOnCreateDTO newAddOn);
    }
}
