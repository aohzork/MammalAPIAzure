using System.Collections.Generic;
using System.Threading.Tasks;
using MammalAPI.DTO;
using MammalAPI.Models;

namespace MammalAPI.Services
{
    public interface IHabitatRepository
    {
        Task<List<IdNameDTO>> GetAllHabitats();
        Task<IdNameDTO> GetHabitatByName(string name);
        Task<Habitat> GetHabitatById(int id);
        
    }
}
