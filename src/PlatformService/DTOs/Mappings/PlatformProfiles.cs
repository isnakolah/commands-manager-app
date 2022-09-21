using AutoMapper;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.DTOs.Mappings;

public class PlatformProfiles : Profile
{
    public PlatformProfiles()
    {
        CreateMap<Platform, PlatformReadDTO>();
        CreateMap<PlatformCreateDTO, Platform>();
    }
}