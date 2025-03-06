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
    private readonly IJwtProvider _jwtProvider;

    public UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        UserRegistrationDtoValidator validator,
        IMapper mapper,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _validator = validator;
        _mapper = mapper;
        _jwtProvider = jwtProvider;
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
    
    public async Task UpdateRefreshTokenAsync(Guid userId, string? refreshToken, DateTime expiryTime)
    {
        await _userRepository.UpdateRefreshTokenAsync(userId, refreshToken, expiryTime);
    }
    
    public async Task<UserDto> GetUserByRefreshTokenAsync(string refreshToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken)
                   ?? throw new UnauthorizedAccessException("Invalid or expired token");

        return _mapper.Map<UserDto>(user);
    }
    
    public async Task<(string AccessToken, string RefreshToken)> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || !_passwordHasher.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");
        
        var accessToken = _jwtProvider.GenerateAccessToken(user);
        var (refreshToken, expiry) = _jwtProvider.GenerateRefreshToken();
        
        await UpdateRefreshTokenAsync(user.Id, refreshToken, expiry);

        return (accessToken, refreshToken);
    }
    
    public async Task RevokeRefreshTokenAsync(Guid userId)
    {
        await _userRepository.UpdateRefreshTokenAsync(userId, null, null);
    }
    public async Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);
        if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid or expired token");
        
        var newAccessToken = _jwtProvider.GenerateAccessToken(user);
        var (newRefreshToken, newExpiry) = _jwtProvider.GenerateRefreshToken();
        
        await UpdateRefreshTokenAsync(user.Id, newRefreshToken, newExpiry);

        return (newAccessToken, newRefreshToken);
    }
}