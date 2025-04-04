using EventManagement.Application.Interfaces;
using EventManagement.Infrastructure.Data;
using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    public async Task<User?> GetByIdAsync(Guid id)  
        => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);


    public async Task UpdateRefreshTokenAsync(User user, string? refreshToken, DateTime? expiryTime)
    {
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = expiryTime;
        await _context.SaveChangesAsync();
    }
    
    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        => await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
}