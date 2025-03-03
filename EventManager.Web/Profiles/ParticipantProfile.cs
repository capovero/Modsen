using AutoMapper;
using EventManager.Domain.Entities;
using EventManager.Web.DTO.Requests;
using EventManager.Web.DTO.Responses;

namespace EventManager.Web.Profiles;

public class ParticipantProfile : Profile
{
    public ParticipantProfile()
    {
        CreateMap<RegisterParticipantRequest, Participant>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrationDate, 
                opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Event, opt => opt.Ignore());
        
        CreateMap<Participant, ParticipantResponse>();
    }
}