using AutoMapper;
using EventManagement.Application.DTO;
using EventManager.Application.DTO;
using EventManager.Web.DTO.Requests;
using EventManager.Web.DTO.Responses;

namespace EventManager.Web.Profiles;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<CreateEventRequest, EventDto>();
        CreateMap<UpdateEventRequest, EventDto>();
        
        CreateMap<EventDto, EventResponse>()
            .ForMember(dest => dest.ParticipantsCount, 
                opt => opt.MapFrom(src => src.Participants.Count));
    }
} 