using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;

    public PlatformsController(IPlatformRepository platformRepository, IMapper mapper)
    {
        _platformRepository = platformRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDTO>>> GetPlatforms()
    {
        var platforms = await _platformRepository.GetAllAsync();

        var platformsReadDTOs = _mapper.Map<IEnumerable<PlatformReadDTO>>(platforms);

        return Ok(platformsReadDTOs);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlatformReadDTO>> GetPlatformById([FromRoute] int id)
    {
        if (await _platformRepository.GetByIdAsync(id) is not {} platform) 
        {
            return NotFound();
        }

        var platformReadDTO = _mapper.Map<PlatformReadDTO>(platform);

        return Ok(platformReadDTO);
    }

    [HttpPost]
    public async Task<ActionResult> CreatePlatform([FromBody] PlatformCreateDTO platformCreateDTO)
    {
        var platform = _mapper.Map<Platform>(platformCreateDTO);

        _platformRepository.Create(platform);

        await _platformRepository.SaveChangesAsync();

        var platformReadDTO = _mapper.Map<PlatformReadDTO>(platform);

        return CreatedAtAction(nameof(GetPlatformById), new {id = platformReadDTO.Id}, platformReadDTO);
    }
}