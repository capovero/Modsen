using AutoMapper;
using EventManager.Domain.Entities;
using EventManager.Web.DTO.Requests;
using EventManager.Web.DTO.Responses;

namespace EventManager.Web.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.MapFrom(_ => "User"))
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore());
        
        CreateMap<User, UserResponse>();
    }
}