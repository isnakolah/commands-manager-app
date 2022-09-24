using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _platformRepository;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMapper _mapper;

    public PlatformsController(IPlatformRepository platformRepository, ICommandDataClient commandDataClient, IMapper mapper)
    {
        _platformRepository = platformRepository;
        _commandDataClient = commandDataClient;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDTO>>> GetPlatforms()
    {
        var platformsReadDTOs = await _platformRepository.GetAllDTOsAsync();

        return Ok(platformsReadDTOs);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlatformReadDTO>> GetPlatformById([FromRoute] int id)
    {
        if (await _platformRepository.GetDTOByIdAsync(id) is not {} platformReadDTO) 
        {
            return NotFound();
        }

        return Ok(platformReadDTO);
    }

    [HttpPost]
    public async Task<ActionResult> CreatePlatform([FromBody] PlatformCreateDTO platformCreateDTO)
    {
        var platform = _mapper.Map<Platform>(platformCreateDTO);

        _platformRepository.Create(platform);

        await _platformRepository.SaveChangesAsync();

        var platformReadDTO = _mapper.Map<PlatformReadDTO>(platform);

        try
        {
            await _commandDataClient.SendPlatformToCommand(platformReadDTO);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
        }

        return CreatedAtAction(nameof(GetPlatformById), new {id = platformReadDTO.Id}, platformReadDTO);
    }
}