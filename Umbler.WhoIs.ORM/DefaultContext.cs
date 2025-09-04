using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Umbler.WhoIs.ORM;

public class DefaultContext(DbContextOptions<DefaultContext> options) : DbContext(options)
{
    /// <summary>
    /// Domain entity DbSet.
    /// </summary>
    public DbSet<Domain.Entities.Domain> Domains { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}

/// <summary>
/// Design-time factory for EF Core tooling (migrations).
/// </summary>
public class YourDbContextFactory : IDesignTimeDbContextFactory<DefaultContext>
{
    /// <summary>
    /// Creates a DefaultContext instance for design-time operations.
    /// </summary>
    /// <param name="args">CLI args.</param>
    /// <returns>Configured DefaultContext.</returns>
    public DefaultContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<DefaultContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseNpgsql(
            connectionString,
            b => b.MigrationsAssembly("Umbler.WhoIs.ORM")
        );
        //.UseSnakeCaseNamingConvention();

        return new DefaultContext(builder.Options);
    }
}