using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepDb
{
    public static void PrepPopulation(this IApplicationBuilder app, bool envIsProd)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

        ApplyMigrations(context, envIsProd);

        SeedData(context);
    }

    private static void ApplyMigrations(AppDbContext context, bool envIsProd)
    {
        if (!envIsProd)
        {
            return;
        }

        Console.WriteLine("--> Attempting to apply migrations...");

        try
        {
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static void SeedData(AppDbContext context)
    {
        if (context.Platforms.Any())
        {
            return;
        }

        context.Platforms.AddRange(
            new Platform {Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
            new Platform {Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free"},
            new Platform {Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free"}
        );

        context.SaveChanges();
    }
}