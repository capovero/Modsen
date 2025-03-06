using AutoMapper;
using EventManager.Application.Profiles;

namespace EventManager.Tests;

public static class TestHelper
{
    public static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => 
        {
            cfg.AddProfile(new EventProfile());
        });
        return config.CreateMapper();
    }
}