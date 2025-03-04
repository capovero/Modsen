using AutoMapper;
using EventManagement.Application.DTO;
using EventManager.Domain.Entities;

namespace EventManager.Application.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
    }
}