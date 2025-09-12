using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Context;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Interfaces;

namespace TideOfDestiniy.DAL.Repositories
{
    public class SystemRequirementRepo : ISystemRequirementRepo
    {
        private readonly TideOfDestinyDbContext _context;

        public SystemRequirementRepo(TideOfDestinyDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateSystemRequirementAsync(SystemRequirement systemRequirement)
        {
            await _context.SystemRequirements.AddAsync(systemRequirement);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSystemRequirementAsync(int id)
        {
            var systemRequirement = _context.SystemRequirements.Find(id);
            if (systemRequirement == null) return false;

            _context.SystemRequirements.Remove(systemRequirement);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SystemRequirement>> GetAllSystemRequirementsAsync() => await _context.SystemRequirements.ToListAsync();

        public async Task<SystemRequirement?> GetSystemRequirementByIdAsync(int id) => await  _context.SystemRequirements.FindAsync(id);

        public async Task<bool> UpdateSystemRequirementAsync(SystemRequirement systemRequirement)
        {
            _context.SystemRequirements.Update(systemRequirement);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
