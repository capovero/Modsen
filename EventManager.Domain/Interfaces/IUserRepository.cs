using EventManager.Domain.Entities;

namespace EventManagement.Application.Interfaces;

public interface IUserRepository
{
    Task<User> GetByEmailAsync(string email);
    Task<User> AddAsync(User user);
    Task<User> UpdateAsync(User user);

    Task<User> GetByIdAsync(Guid id);

    Task UpdateRefreshTokenAsync(Guid userId, string? refreshToken, DateTime? expiryTime);

    Task<User?> GetByRefreshTokenAsync(string refreshToken);
}