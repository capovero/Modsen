using AutoMapper;
using EventManagement.Application.DTO;
using EventManager.Domain.Entities;

namespace EventManager.Application.Profiles;

public class ParticipantProfile : Profile
{
    public ParticipantProfile()
    {
        CreateMap<ParticipantDto, Participant>().ReverseMap();
    }
}