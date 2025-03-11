using AutoMapper;
using EventManagement.Application.Interfaces;
using EventManagement.Application.Validators;
using EventManager.Application.DTO;
using EventManager.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Application.Services;

public class EventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly EventDtoValidator _validator;

    public EventService(
        IEventRepository eventRepository,
        IMapper mapper,
        EventDtoValidator validator)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<EventDto> CreateEventAsync(EventDto eventDto)
    {
        await _validator.ValidateAndThrowAsync(eventDto);
        var newEvent = _mapper.Map<Event>(eventDto);
        await _eventRepository.AddAsync(newEvent);
        return _mapper.Map<EventDto>(newEvent);
    }

    public async Task<EventDto> UpdateEventAsync(Guid id, EventDto eventDto)
    {
        var existingEvent = await _eventRepository.GetByIdAsync(id) 
                            ?? throw new KeyNotFoundException($"Event {id} not found");

        await _validator.ValidateAndThrowAsync(eventDto);
        
        existingEvent.Title = eventDto.Title;
        existingEvent.Description = eventDto.Description;
        existingEvent.Date = eventDto.Date;
        existingEvent.Location = eventDto.Location;
        existingEvent.Category = eventDto.Category;
        existingEvent.MaxParticipants = eventDto.MaxParticipants;
        
        if (!string.IsNullOrEmpty(eventDto.ImageUrl))
            existingEvent.ImageUrl = eventDto.ImageUrl;

        await _eventRepository.UpdateAsync(existingEvent);
        return _mapper.Map<EventDto>(existingEvent);
    }

    public async Task DeleteEventAsync(Guid id)
    {
        var eventToDelete = await _eventRepository.GetByIdAsync(id) 
                            ?? throw new KeyNotFoundException($"Event {id} not found");
        await _eventRepository.DeleteAsync(eventToDelete);
    }

    public async Task<EventDto> GetEventByIdAsync(Guid id)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(id) 
                          ?? throw new KeyNotFoundException($"Event {id} not found");
        return _mapper.Map<EventDto>(eventEntity);
    }
    public async Task<List<EventDto>> GetEventsByCriteriaAsync(
        string? title,
        DateTime? date,
        string? location,
        string? category,
        int pageNumber = 1,
        int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
            throw new ArgumentException("Page number and size must be greater than 0.");

        var query = _eventRepository.GetQueryable();

        if (!string.IsNullOrEmpty(title))
            query = query.Where(e => e.Title.Contains(title));
        
        if (date.HasValue)
            query = query.Where(e => e.Date.Date == date.Value.Date);
        
        if (!string.IsNullOrEmpty(location))
            query = query.Where(e => e.Location == location);
        
        if (!string.IsNullOrEmpty(category))
            query = query.Where(e => e.Category == category);

        return _mapper.Map<List<EventDto>>(
            await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
        );
    }
}