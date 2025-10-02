using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs.Requests;
using TideOfDestiniy.BLL.DTOs.Responses;

namespace TideOfDestiniy.BLL.Interfaces
{
    public interface ISystemRequirementService
    {
        Task<AuthResultDTO> CreateSystemRequirementAsync(CreateSystemRequirementDTO systemRequirement);
        Task<SystemRequirementDTO?> GetSystemRequirementByIdAsync(int id);
        Task<List<SystemRequirementDTO>> GetAllSystemRequirementsAsync();
        Task<AuthResultDTO> UpdateSystemRequirementAsync(UpdateSystemRequirementDTO systemRequirement, int id);
        Task<AuthResultDTO> DeleteSystemRequirementAsync(int id);
    }
}
