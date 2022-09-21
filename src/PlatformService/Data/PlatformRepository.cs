using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public sealed class PlatformRepository : IPlatformRepository
{
    private readonly AppDbContext _context;

    public PlatformRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Platform>> GetAllAsync()
    {
        return await _context.Platforms.ToArrayAsync();
    }

    public async Task<Platform?> GetByIdAsync(int id)
    {
        return await _context.Platforms.FindAsync(id);
    }

    public void Create(Platform platform)
    {
        _context.Platforms.Add(platform);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}