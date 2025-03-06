using AutoMapper;
using EventManager.Web.DTO.Requests;
using EventManager.Web.DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EventManagement.Application.DTO;
using EventManager.Application.Services;

namespace EventManager.Web.Controllers;

[ApiController]
[Route("api/participants")]
[Authorize]
public class ParticipantsController : ControllerBase
{
    private readonly ParticipantService _participantService;
    private readonly IMapper _mapper;

    public ParticipantsController(ParticipantService participantService, IMapper mapper)
    {
        _participantService = participantService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<ParticipantResponse>> Register([FromBody] RegisterParticipantRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var participantDto = _mapper.Map<ParticipantDto>(request);
        participantDto.UserId = userId;
        var createdParticipant = await _participantService.RegisterParticipantAsync(participantDto);
        return Ok(_mapper.Map<ParticipantResponse>(createdParticipant));
    }
    
    [HttpGet("event/{eventId}")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<ActionResult<List<ParticipantResponse>>> GetByEventId(Guid eventId)
    {
        var participants = await _participantService.GetParticipantsByEventIdAsync(eventId);
        return Ok(_mapper.Map<List<ParticipantResponse>>(participants));
    }
    
    [HttpGet("{participantId}")]
    public async Task<ActionResult<ParticipantResponse>> GetById(Guid participantId)
    {
        var participantDto = await _participantService.GetParticipantByIdAsync(participantId);
        return Ok(_mapper.Map<ParticipantResponse>(participantDto));
    }
    
    [HttpDelete("{participantId}")]
    public async Task<ActionResult> CancelRegistration(Guid participantId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        await _participantService.CancelRegistrationAsync(userId, participantId);
        return NoContent();
    }
}