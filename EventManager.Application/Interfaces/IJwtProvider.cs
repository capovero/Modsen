using EventManager.Domain.Entities;

namespace EventManagement.Application.Interfaces;

public interface IJwtProvider
{
    string GenerateAccessToken(User user);
    (string Token, DateTime Expiry) GenerateRefreshToken();
}