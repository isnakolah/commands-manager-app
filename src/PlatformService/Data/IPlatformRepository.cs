using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Data;

public interface IPlatformRepository
{
    Task<IEnumerable<Platform>> GetAllAsync();
    Task<IEnumerable<PlatformReadDTO>> GetAllDTOsAsync();
    Task<Platform?> GetByIdAsync(int id);
    Task<PlatformReadDTO?> GetDTOByIdAsync(int id);
    void Create(Platform platform);
    Task<bool> SaveChangesAsync();
}