using AutoMapper;
using EventManager.Application.DTO;
using EventManager.Application.Services;
using EventManager.Web.DTO.Requests;
using EventManager.Web.DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Web.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly EventService _eventService;
    private readonly IMapper _mapper;

    public EventsController(EventService eventService, IMapper mapper)
    {
        _eventService = eventService;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<EventResponse>>> GetAll(
        [FromQuery] string? title,
        [FromQuery] DateTime? date,
        [FromQuery] string? location,
        [FromQuery] string? category,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var events = await _eventService.GetEventsByCriteriaAsync(title, date, location, category, page, pageSize);
        return Ok(_mapper.Map<List<EventResponse>>(events));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<EventResponse>> GetById(Guid id)
    {
        var eventDto = await _eventService.GetEventByIdAsync(id);
        return Ok(_mapper.Map<EventResponse>(eventDto));
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<EventResponse>> Create([FromForm] CreateEventRequest request)
    {
        var eventDto = _mapper.Map<EventDto>(request);
        var createdEvent = await _eventService.CreateEventAsync(eventDto);
        return CreatedAtAction(nameof(GetById), new { id = createdEvent.Id }, _mapper.Map<EventResponse>(createdEvent));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<EventResponse>> Update(Guid id, [FromForm] UpdateEventRequest request)
    {
        var eventDto = _mapper.Map<EventDto>(request);
        var updatedEvent = await _eventService.UpdateEventAsync(id, eventDto);
        return Ok(_mapper.Map<EventResponse>(updatedEvent));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _eventService.DeleteEventAsync(id);
        return NoContent();
    }
}