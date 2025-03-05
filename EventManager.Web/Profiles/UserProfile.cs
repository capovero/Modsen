using AutoMapper;
using EventManagement.Application.DTO;
using EventManager.Web.DTO.Requests;
using EventManager.Web.DTO.Responses;

namespace EventManager.Web.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserRequest, UserRegistrationDto>();
        
        CreateMap<UserDto, UserResponse>();
    }
}