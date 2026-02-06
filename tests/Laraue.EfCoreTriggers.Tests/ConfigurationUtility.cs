using System;
using Microsoft.Extensions.Configuration;

namespace Laraue.EfCoreTriggers.Tests;

public class ConfigurationUtility
{
    public static string GetConnectionString(string name)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();

        return configuration.Build()
            .GetConnectionString(name)
            ?? throw new InvalidOperationException($"Could not find connection string for key {name}");
    }
}