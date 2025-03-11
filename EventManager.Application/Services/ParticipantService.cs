using AutoMapper;
using EventManagement.Application.DTO;
using EventManagement.Application.Interfaces;
using EventManagement.Application.Validators;
using EventManager.Domain.Entities;
using FluentValidation;

namespace EventManager.Application.Services;

public class ParticipantService
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly ParticipantDtoValidator _validator;

    public ParticipantService(
        IParticipantRepository participantRepository,
        IEventRepository eventRepository,
        IMapper mapper,
        ParticipantDtoValidator validator)
    {
        _participantRepository = participantRepository;
        _eventRepository = eventRepository;
        _mapper = mapper;
        _validator = validator;
    }
    public async Task<ParticipantDto> GetParticipantByIdAsync(Guid participantId)
    {
        var participant = await _participantRepository.GetByIdAsync(participantId) 
                          ?? throw new KeyNotFoundException("Participant not found");
        return _mapper.Map<ParticipantDto>(participant);
    }

    public async Task<ParticipantDto> RegisterParticipantAsync(ParticipantDto participantDto)
    {
        await _validator.ValidateAndThrowAsync(participantDto);

        var eventEntity = await _eventRepository.GetByIdAsync(participantDto.EventId) 
                          ?? throw new KeyNotFoundException("Event not found");
    
        var participantsCount = await _participantRepository.CountByEventIdAsync(eventEntity.Id);
        if (participantsCount >= eventEntity.MaxParticipants)
            throw new InvalidOperationException("No available slots");
    
        var participant = _mapper.Map<Participant>(participantDto);
        await _participantRepository.AddAsync(participant);
        return _mapper.Map<ParticipantDto>(participant);
    }

    public async Task CancelRegistrationAsync(Guid userId, Guid participantId)
    {
        var participant = await _participantRepository.GetByIdAsync(participantId) 
                          ?? throw new KeyNotFoundException("Participant not found");
    
        if (participant.UserId != userId)
            throw new UnauthorizedAccessException("You cannot cancel another user's registration");
    
        await _participantRepository.RemoveAsync(participant);
    }

    public async Task<List<ParticipantDto>> GetParticipantsByEventIdAsync(Guid eventId)
    {
        var participants = await _participantRepository.GetByEventIdAsync(eventId);
        return _mapper.Map<List<ParticipantDto>>(participants);
    }
}