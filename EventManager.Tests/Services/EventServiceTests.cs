using AutoMapper;
using EventManagement.Application.Validators;
using EventManager.Application.DTO;
using EventManager.Application.Services;
using EventManager.Domain.Entities;
using EventManagement.Infrastructure.Data;
using EventManagement.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventManager.Tests.Services;

public class EventServiceTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly EventService _service;

    public EventServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        var repository = new EventRepository(_context);
        var mapper = TestHelper.CreateMapper();
        var validator = new EventDtoValidator();

        _service = new EventService(repository, mapper, validator);
    }

    
    // public EventService(
    //     IEventRepository eventRepository,
    //     IParticipantRepository participantRepository,
    //     IMapper mapper,
    //     EventDtoValidator validator)
    // {
    //     _eventRepository = eventRepository;
    //     _participantRepository = participantRepository;
    //     _mapper = mapper;
    //     _validator = validator;
    // }
    
    [Fact]
    public async Task CreateEventAsync_ShouldCreateEventWithAllFields()
    {
        var eventDto = new EventDto 
        { 
            Title = "Концерт",
            Description = "Концерт известной группы",
            Date = DateTime.UtcNow.AddDays(7),
            Location = "Москва, стадион Лужники",
            Category = "Музыка",
            MaxParticipants = 5000,
            ImageUrl = "/uploads/concert.jpg"
        };

        var result = await _service.CreateEventAsync(eventDto);

        Assert.NotNull(result);
        Assert.Equal("Концерт", result.Title);
        Assert.Equal("Музыка", result.Category);
        Assert.Equal(5000, result.MaxParticipants);
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldUpdateAllFields()
    {
        var existingEvent = await _service.CreateEventAsync(new EventDto 
        { 
            Title = "Старое название",
            Description = "Старое описание",
            Date = DateTime.UtcNow.AddDays(1),
            Location = "Санкт-Петербург",
            Category = "Искусство",
            MaxParticipants = 100,
            ImageUrl = "/uploads/old.jpg"
        });

        var updatedDto = new EventDto 
        { 
            Title = "Новое название",
            Description = "Новое описание",
            Date = DateTime.UtcNow.AddDays(14),
            Location = "Казань",
            Category = "Театр",
            MaxParticipants = 200,
            ImageUrl = "/uploads/new.jpg"
        };

        var result = await _service.UpdateEventAsync(existingEvent.Id, updatedDto);

        Assert.Equal("Новое название", result.Title);
        Assert.Equal("Новое описание", result.Description);
        Assert.Equal("Казань", result.Location);
    }

    [Fact]
    public async Task DeleteEventAsync_ShouldRemoveEventFromDatabase()
    {
        var eventDto = new EventDto 
        { 
            Title = "Событие для удаления",
            Description = "Описание",
            Date = DateTime.UtcNow,
            Location = "Екатеринбург",
            Category = "Тест",
            MaxParticipants = 10,
            ImageUrl = "/uploads/test.jpg"
        };
        var createdEvent = await _service.CreateEventAsync(eventDto);

        await _service.DeleteEventAsync(createdEvent.Id);

        await Assert.ThrowsAsync<KeyNotFoundException>(() 
            => _service.GetEventByIdAsync(createdEvent.Id));
    }

    [Fact]
    public async Task GetEventByIdAsync_ShouldReturnEventWithCorrectData()
    {
        var eventDto = new EventDto 
        { 
            Title = "Тестовое событие",
            Description = "Описание",
            Date = DateTime.UtcNow,
            Location = "Новосибирск",
            Category = "Тест",
            MaxParticipants = 50,
            ImageUrl = "/uploads/test.jpg"
        };
        var createdEvent = await _service.CreateEventAsync(eventDto);

        var result = await _service.GetEventByIdAsync(createdEvent.Id);

        Assert.Equal("Новосибирск", result.Location);
        Assert.Equal(50, result.MaxParticipants);
    }

    [Fact]
    public async Task GetEventsByCriteriaAsync_ShouldFilterByCategory()
    {
        await _service.CreateEventAsync(new EventDto 
        { 
            Title = "Концерт", 
            Description = "Описание",
            Date = DateTime.UtcNow,
            Location = "Москва",
            Category = "Музыка",
            MaxParticipants = 1000,
            ImageUrl = "/uploads/music.jpg"
        });

        await _service.CreateEventAsync(new EventDto 
        { 
            Title = "Конференция", 
            Description = "Описание",
            Date = DateTime.UtcNow,
            Location = "Санкт-Петербург",
            Category = "IT",
            MaxParticipants = 500,
            ImageUrl = "/uploads/it.jpg"
        });

        var results = await _service.GetEventsByCriteriaAsync(
            title: null,
            date: null,
            location: null,
            category: "Музыка",
            pageNumber: 1,
            pageSize: 10
        );

        Assert.Single(results);
        Assert.Equal("Концерт", results[0].Title);
    }

    public void Dispose() => _context.Database.EnsureDeleted();
}