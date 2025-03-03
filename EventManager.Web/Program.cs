using EventManagement.Application.Interfaces;
using Microsoft.OpenApi.Models;
using EventManagement.Infrastructure.Data;
using EventManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using EventManager.Domain.Entities;
using AutoMapper;
using EventManager.Web.Profiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAutoMapper(typeof(EventProfile), typeof(ParticipantProfile), typeof(UserProfile));


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();

// builder.Services.AddScoped<PasswordHasher>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();