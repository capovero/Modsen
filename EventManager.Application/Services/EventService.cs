using AutoMapper;
using EventManagement.Application.Interfaces;
using EventManagement.Application.Validators;
using EventManager.Application.DTO;
using EventManager.Domain.Entities;
using FluentValidation;

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
        
        _mapper.Map(eventDto, existingEvent);
        await _eventRepository.UpdateAsync(existingEvent);
        return _mapper.Map<EventDto>(existingEvent);
    }

    public async Task DeleteEventAsync(Guid id)
    {
        var eventToDelete = await _eventRepository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException($"Event {id} not found");
        
        await _eventRepository.DeleteAsync(id);
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
        var events = await _eventRepository.GetByCriteriaAsync(
            title, date, location, category, pageNumber, pageSize
        );
        
        return _mapper.Map<List<EventDto>>(events);
    }
}