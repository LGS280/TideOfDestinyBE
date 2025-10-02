using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TideOfDestiniy.BLL.DTOs.Requests;
using TideOfDestiniy.BLL.Interfaces;

namespace TideOfDestiniy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemRequirementController : ControllerBase
    {
        private ISystemRequirementService _requirementService;

        public SystemRequirementController(ISystemRequirementService requirementService)
        {
            _requirementService = requirementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSystemRequirements()
        {
            var requirements = await _requirementService.GetAllSystemRequirementsAsync();
            return Ok(requirements);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetSystemRequirementById(int id)
        {
            var requirement = await _requirementService.GetSystemRequirementByIdAsync(id);
            if (requirement == null)
            {
                return NotFound(new { message = "System Requirement not found." });
            }
            return Ok(requirement);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSystemRequirement([FromBody] CreateSystemRequirementDTO requirementDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _requirementService.CreateSystemRequirementAsync(requirementDTO);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateSystemRequirement([FromBody] UpdateSystemRequirementDTO requirementDTO, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var requirementExists = await _requirementService.GetSystemRequirementByIdAsync(id);
            if (requirementExists == null)
            {
                return NotFound(new { message = "System Requirement not found." });
            }


            var result = await _requirementService.UpdateSystemRequirementAsync(requirementDTO, id);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteSystemRequirement(int id)
        {
            var result = await _requirementService.DeleteSystemRequirementAsync(id);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }
    }
}
