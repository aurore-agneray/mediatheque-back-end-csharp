using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;
using Infrastructure.BnfApi.Repositories;
using Infrastructure.MySQL;
using Infrastructure.MySQL.ComplexRequests;
using MediathequeBackCSharp.Managers.SearchManagers;
using MediathequeBackCSharp.Services;
using MediathequeBackCSharp.Services.Aggregates;
using MediathequeBackCSharp.Texts;
using Microsoft.EntityFrameworkCore;

namespace MediathequeBackCSharp.Configuration;

/// <summary>
/// Contains methods for implementing dependency injection into the startup class
/// </summary>
public static class StartUpDI
{
    /// <summary>
    /// Injects the needed databases' contexts
    /// </summary>
    /// <param name="builder">The application builder</param>
    public static void InjectDbContexts(WebApplicationBuilder builder)
    {
        if (builder is null)
        {
            return;
        }

        // Add the connection to the MySQL database
        builder.Services.AddDbContext<MySQLDbContext>(optionsBuilder =>
        {
            MySQLDatabaseSettings dbSettings = new();

            if (string.IsNullOrEmpty(dbSettings.DbConnectionString))
            {
                throw new ArgumentException(InternalErrorTexts.ERROR_MISSING_CONNEXION_STRING);
            }

            optionsBuilder.UseMySql(
                dbSettings.DbConnectionString,
                ServerVersion.AutoDetect(dbSettings.DbConnectionString)
            );
        });
    }

    /// <summary>
    /// Injects the repositories which providing data
    /// from any source
    /// </summary>
    /// <param name="builder">The application builder</param>
    public static void InjectRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IIdentifiedRepository<Author>, MySQLIIdentifiedRepository<Author>>();
        builder.Services.AddScoped<IIdentifiedRepository<Book>, MySQLIIdentifiedRepository<Book>>();
        builder.Services.AddScoped<IIdentifiedRepository<Edition>, MySQLIIdentifiedRepository<Edition>>();
        builder.Services.AddScoped<IIdentifiedRepository<Format>, MySQLIIdentifiedRepository<Format>>();
        builder.Services.AddScoped<IIdentifiedRepository<Genre>, MySQLIIdentifiedRepository<Genre>>();
        builder.Services.AddScoped<IIdentifiedRepository<Publisher>, MySQLIIdentifiedRepository<Publisher>>();
        builder.Services.AddScoped<IIdentifiedRepository<Series>, MySQLIIdentifiedRepository<Series>>();

        builder.Services.AddScoped<MySQLSimpleSearchRepository>();
        builder.Services.AddScoped<MySQLAdvancedSearchRepository>();

        builder.Services.AddScoped<BnfApiSimpleSearchRepository>();
    }

    /// <summary>
    /// Injects the services which call the available repositories
    /// </summary>
    /// <param name="builder">The application builder</param>
    public static void InjectServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<MySQLSimpleSearchService>();
        builder.Services.AddScoped<MySQLAdvancedSearchService>();

        builder.Services.AddScoped<BnfApiSimpleSearchService>();

        builder.Services.AddScoped<AllSimpleSearchServices>();
        builder.Services.AddScoped<AllAdvancedSearchServices>();
    }

    /// <summary>
    /// Injects the managers which call the appropriate services
    /// </summary>
    /// <param name="builder">The application builder</param>
    public static void InjectManagers(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<SimpleSearchManager>();
        builder.Services.AddScoped<AdvancedSearchManager>();
    }
}