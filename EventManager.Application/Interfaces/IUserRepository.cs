using EventManager.Domain.Entities;

namespace EventManagement.Application.Interfaces;

public interface IUserRepository
{
    Task<User> GetByEmailAsync(string email);
    Task<User> AddAsync(User user);
    Task<User> UpdateAsync(User user);
    // не забыть про рефреш токен
}