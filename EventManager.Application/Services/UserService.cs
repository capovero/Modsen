// EventManager.Application/Services/UserService.cs

using AutoMapper;
using EventManagement.Application.DTO;
using EventManagement.Application.Interfaces;
using EventManagement.Application.Validators;
using EventManager.Domain.Entities;
using FluentValidation;

namespace EventManager.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly UserRegistrationDtoValidator _validator;
    private readonly IMapper _mapper;

    public UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        UserRegistrationDtoValidator validator,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<UserDto> RegisterUserAsync(UserRegistrationDto registrationDto)
    {
        await _validator.ValidateAndThrowAsync(registrationDto);
        
        var hashedPassword = _passwordHasher.Generate(registrationDto.Password);
        
        var user = new User
        {
            Email = registrationDto.Email,
            PasswordHash = hashedPassword,
            Role = "User"
        };
        
        await _userRepository.AddAsync(user);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId) 
                   ?? throw new KeyNotFoundException("User not found");
        
        return _mapper.Map<UserDto>(user);
    }
}