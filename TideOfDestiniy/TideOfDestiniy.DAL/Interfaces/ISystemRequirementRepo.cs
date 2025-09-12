using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.DAL.Interfaces
{
    public interface ISystemRequirementRepo
    {
        Task<bool> CreateSystemRequirementAsync(SystemRequirement systemRequirement);
        Task<SystemRequirement?> GetSystemRequirementByIdAsync(int id);
        Task<List<SystemRequirement>> GetAllSystemRequirementsAsync();
        Task<bool> UpdateSystemRequirementAsync(SystemRequirement systemRequirement);
        Task<bool> DeleteSystemRequirementAsync(int id);
    }
}
