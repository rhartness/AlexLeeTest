using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EntityFrameworkProject;
using System.IO;

public static class DbContextFactory
{
    public static AlexLeeDbContext CreateDbContext()
    {
        // Build configuration from the appsettings.json file
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Configure DbContext options
        var optionsBuilder = new DbContextOptionsBuilder<AlexLeeDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        // Create and return the DbContext instance
        return new AlexLeeDbContext(optionsBuilder.Options);
    }
}