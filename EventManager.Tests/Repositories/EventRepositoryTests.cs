using EventManager.Domain.Entities;
using EventManagement.Infrastructure.Data;
using EventManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventManager.Tests.Repositories;

public class EventRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly EventRepository _repository;

    public EventRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new EventRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldSaveFullEventData()
    {
        var newEvent = new Event 
        { 
            Title = "Тестовое событие",
            Description = "Полное описание",
            Date = DateTime.UtcNow,
            Location = "Москва",
            Category = "Тест",
            MaxParticipants = 100,
            ImageUrl = "/uploads/image.jpg"
        };

        var result = await _repository.AddAsync(newEvent);

        Assert.Equal("Москва", result.Location);
        Assert.Equal(100, result.MaxParticipants);
        Assert.Equal("/uploads/image.jpg", result.ImageUrl);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEventWithAllFields()
    {
        var newEvent = new Event 
        { 
            Title = "Событие",
            Description = "Описание",
            Date = DateTime.UtcNow.AddDays(3),
            Location = "Казань",
            Category = "Культура",
            MaxParticipants = 200,
            ImageUrl = "/uploads/culture.jpg"
        };
        await _repository.AddAsync(newEvent);

        var result = await _repository.GetByIdAsync(newEvent.Id);

        Assert.Equal("Казань", result.Location);
        Assert.Equal(200, result.MaxParticipants);
        Assert.Equal("Культура", result.Category);
    }

    public void Dispose() => _context.Database.EnsureDeleted();
}