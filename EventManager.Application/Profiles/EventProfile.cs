using AutoMapper;
using EventManagement.Application.DTO;
using EventManager.Application.DTO;
using EventManager.Domain.Entities;

namespace EventManager.Application.Profiles;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<EventDto, Event>().ReverseMap();
    }
}