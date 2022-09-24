using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PlatformService.DTOs;
using PlatformService.Models;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace PlatformService.Data;

public sealed class PlatformRepository : IPlatformRepository
{
    private readonly IConfigurationProvider _mapperConfig;
    private readonly AppDbContext _context;

    public PlatformRepository(IConfigurationProvider mapperConfig, AppDbContext context)
    {
        _mapperConfig = mapperConfig;
        _context = context;
    }

    public async Task<IEnumerable<Platform>> GetAllAsync()
    {
        return await _context.Platforms.ToArrayAsync();
    }

    public async Task<IEnumerable<PlatformReadDTO>> GetAllDTOsAsync()
    {
        var platformDTOs = await _context.Platforms
            .ProjectTo<PlatformReadDTO>(_mapperConfig)
            .ToArrayAsync();

        return platformDTOs;
    }

    public async Task<Platform?> GetByIdAsync(int id)
    {
        return await _context.Platforms.FindAsync(id);
    }

    public async Task<PlatformReadDTO?> GetDTOByIdAsync(int id)
    {
        var platformDTO = await _context.Platforms
            .Where(x => x.Id == id)
            .ProjectTo<PlatformReadDTO>(_mapperConfig)
            .FirstOrDefaultAsync();

        return platformDTO;
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