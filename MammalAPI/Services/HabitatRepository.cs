using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MammalAPI.Context;
using MammalAPI.DTO;
using MammalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MammalAPI.Services
{
    public class HabitatRepository : Repository, IHabitatRepository
    {
        public HabitatRepository(DBContext DBContext, ILogger<FamilyRepository> logger) : base(DBContext, logger)
        { }

        public async Task<List<FamilyDTO>> GetAllHabitats()
        {
            _logger.LogInformation("Getting all habitats");
            var query = _dBContext.Habitats
                .Select(x => new FamilyDTO
                {
                    FamilyID = x.HabitatID,
                    Name = x.Name
                });

            return await query.ToListAsync();
        }

        public async Task<FamilyDTO> GetHabitatByName(string name)
        {
            _logger.LogInformation($"Getting habitat with name: { name }");
            var query = await _dBContext.Habitats
                .Where(h => h.Name == name)
                .Select(s => new FamilyDTO
                {
                    Name = s.Name,
                    FamilyID = s.HabitatID
                })
                .FirstOrDefaultAsync();
            
            if (query == null) throw new System.Exception($"Not found {name}");
            
            return  query;
        }

        public async Task<HabitatDTO> GetHabitatById(int id)
        {
            _logger.LogInformation($"Getting habitat with id: { id }");
            var query = _dBContext.Habitats
                .Where(x => x.HabitatID == id)
                .Select(x => new HabitatDTO
                {
                    HabitatID = x.HabitatID,
                    Name = x.Name
                });

            if (query == null) throw new System.Exception($"Not found {id}");

            return await query.FirstOrDefaultAsync();
        }
    }
}
