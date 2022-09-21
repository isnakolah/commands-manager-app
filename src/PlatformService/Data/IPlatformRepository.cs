using PlatformService.Models;

namespace PlatformService.Data;

public interface IPlatformRepository
{
    Task<IEnumerable<Platform>> GetAllAsync();
    Task<Platform?> GetByIdAsync(int id);
    void Create(Platform platform);
    Task<bool> SaveChangesAsync();
}