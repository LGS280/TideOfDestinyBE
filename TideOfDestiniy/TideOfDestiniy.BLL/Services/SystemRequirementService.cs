using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs.Requests;
using TideOfDestiniy.BLL.DTOs.Responses;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.DAL.Interfaces;

namespace TideOfDestiniy.BLL.Services
{
    public class SystemRequirementService : ISystemRequirementService
    {
        private ISystemRequirementRepo _requirementRepo;

        public SystemRequirementService(ISystemRequirementRepo requirementRepo)
        {
            _requirementRepo = requirementRepo;
        }
        public async Task<AuthResultDTO> CreateSystemRequirementAsync(CreateSystemRequirementDTO systemRequirement)
        {
            var newRequirement = new DAL.Entities.SystemRequirement
            {
                OS = systemRequirement.OS,
                Processor = systemRequirement.Processor,
                Memory = systemRequirement.Memory,
                Graphics = systemRequirement.Graphics,
                DirectXVersion = systemRequirement.DirectX,
                Storage = systemRequirement.Storage,
                Type = systemRequirement.Type.ToLower() == "minimum" ? DAL.Entities.RequirementType.Minimum : DAL.Entities.RequirementType.Recommended
            };

            var create = await _requirementRepo.CreateSystemRequirementAsync(newRequirement);

            if (!create)
            {
                return new AuthResultDTO { Succeeded = false, Message = "Failed to create system requirement." };
            }

            return new AuthResultDTO { Succeeded = true, Message = "System requirement created successfully." };

        }

        public async Task<AuthResultDTO> DeleteSystemRequirementAsync(int id)
        {
            var delete = await _requirementRepo.DeleteSystemRequirementAsync(id);
            if (!delete)
            {
                return new AuthResultDTO { Succeeded = false, Message = "System requirement not found." };
            }

            await _requirementRepo.DeleteSystemRequirementAsync(id);
            return new AuthResultDTO { Succeeded = true, Message = "System requirement deleted successfully." };

        }

        public async Task<List<SystemRequirementDTO>> GetAllSystemRequirementsAsync()
        {
            var requirements = await _requirementRepo.GetAllSystemRequirementsAsync();
            return requirements.Select(r => new SystemRequirementDTO
            {
                Id = r.Id,
                OS = r.OS,
                Processor = r.Processor,
                Memory = r.Memory,
                Graphics = r.Graphics,
                DirectX = r.DirectXVersion,
                Storage = r.Storage,
                Type = r.Type.ToString()
            }).ToList();


        }

        public async Task<SystemRequirementDTO?> GetSystemRequirementByIdAsync(int id)
        {
            var requirement =  await _requirementRepo.GetSystemRequirementByIdAsync(id);
            if (requirement == null) return null;

            return new SystemRequirementDTO
            {
                Id = requirement.Id,
                OS = requirement.OS,
                Processor = requirement.Processor,
                Memory = requirement.Memory,
                Graphics = requirement.Graphics,
                DirectX = requirement.DirectXVersion,
                Storage = requirement.Storage,
                Type = requirement.Type.ToString()
            };
        }

        public async Task<AuthResultDTO> UpdateSystemRequirementAsync(UpdateSystemRequirementDTO systemRequirement, int id)
        {
            var existingRequirementTask = await _requirementRepo.GetSystemRequirementByIdAsync(id);
            if (existingRequirementTask == null)
            {
                return new AuthResultDTO { Succeeded = false, Message = "System requirement not found." };
            }

            existingRequirementTask.OS = systemRequirement.OS;
            existingRequirementTask.Processor = systemRequirement.Processor;
            existingRequirementTask.Memory = systemRequirement.Memory;
            existingRequirementTask.Graphics = systemRequirement.Graphics;
            existingRequirementTask.DirectXVersion = systemRequirement.DirectX;
            existingRequirementTask.Storage = systemRequirement.Storage;
            existingRequirementTask.Type = systemRequirement.Type.ToLower() == "minimum" ? DAL.Entities.RequirementType.Minimum : DAL.Entities.RequirementType.Recommended;

            var update = await _requirementRepo.UpdateSystemRequirementAsync(existingRequirementTask);
            if (!update)
            {
                return new AuthResultDTO { Succeeded = false, Message = "Failed to update system requirement." };
            }

            return new AuthResultDTO { Succeeded = true, Message = "System requirement updated successfully." };
        }
    }
}
