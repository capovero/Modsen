using AutoMapper;
using EventManager.Domain.Entities;
using EventManager.Web.DTO.Requests;
using EventManager.Web.DTO.Responses;

namespace EventManager.Web.Profiles;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<CreateEventRequest, Event>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Participants, opt => opt.Ignore());
        
        CreateMap<Event, EventResponse>()
            .ForMember(dest => dest.ParticipantsCount, 
                opt => opt.MapFrom(src => src.Participants.Count));
    }
}