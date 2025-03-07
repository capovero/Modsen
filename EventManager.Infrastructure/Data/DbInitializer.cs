using EventManagement.Application.Interfaces;
using EventManagement.Infrastructure.Data;
using EventManager.Domain.Entities;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context, IPasswordHasher passwordHasher)
    {
        if (context.Users.Any()) return; // Если есть пользователи - пропускаем

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Email = "administrator@example.com",
            Role = "Admin",
            PasswordHash = passwordHasher.Generate("AdminPassword123"),
            RefreshTokenExpiry = DateTime.UtcNow.AddYears(1)
        };

        context.Users.Add(admin);
        context.SaveChanges();
    }
}