using AutoMapper;
using EventManagement.Application.DTO;
using EventManager.Web.DTO.Requests;
using EventManager.Web.DTO.Responses;

namespace EventManager.Web.Profiles;

public class ParticipantProfile : Profile
{
    public ParticipantProfile()
    {
        CreateMap<RegisterParticipantRequest, ParticipantDto>();
        
        CreateMap<ParticipantDto, ParticipantResponse>();
    }
}