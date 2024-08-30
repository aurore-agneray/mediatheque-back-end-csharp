using ApplicationCore.AutoMapper;
using ApplicationCore.DatabasesSettings;
using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;
using ApplicationCore.Services;
using Infrastructure.MySQL;
using mediatheque_back_csharp.Configuration;
using mediatheque_back_csharp.Managers.SearchManagers;
using mediatheque_back_csharp.Middlewares;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Resources;

var routePrefix = "/api";

// Creates a dependency injection container
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add repositories for the MySQL database
builder.Services.AddScoped<IIdentifiedRepository<Author>, MySQLIIdentifiedRepository<Author>>();
builder.Services.AddScoped<IIdentifiedRepository<Book>, MySQLIIdentifiedRepository<Book>>();
builder.Services.AddScoped<IIdentifiedRepository<Edition>, MySQLIIdentifiedRepository<Edition>>();
builder.Services.AddScoped<IIdentifiedRepository<Format>, MySQLIIdentifiedRepository<Format>>();
builder.Services.AddScoped<IIdentifiedRepository<Genre>, MySQLIIdentifiedRepository<Genre>>();
builder.Services.AddScoped<IIdentifiedRepository<Publisher>, MySQLIIdentifiedRepository<Publisher>>();
builder.Services.AddScoped<IIdentifiedRepository<Series>, MySQLIIdentifiedRepository<Series>>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(StartUpOptions.GetSwaggerGenOptions(routePrefix));

// Add the connection to the database
builder.Services.AddScoped<IDatabaseSettings, MySQLDatabaseSettings>();
builder.Services.AddDbContext<IMediathequeDbContext<MySQLDatabaseSettings>, MySQLDbContext>(optionsBuilder =>
{
    MySQLDatabaseSettings dbSettings = new();

    if (string.IsNullOrEmpty(dbSettings.DbConnectionString))
    {
        throw new ArgumentNullException("Please insert the Connection String into the database's settings !");
    }

    optionsBuilder.UseMySql(
        dbSettings.DbConnectionString,
        ServerVersion.AutoDetect(dbSettings.DbConnectionString)
    );
});

// Retrieves the configuration settings entered into Infrastructure.MySQL project
var mySQLSettings = new MySQLDatabaseSettings();

// Configures CORS policy
builder.Services.AddCors(StartUpOptions.GetCorsOptions(mySQLSettings));

// Configures AutoMapper used for converting my POCOs into DTOs
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
});

// Injects texts resources
builder.Services.AddScoped<ResourceManager>(provider => 
    new ResourceManager(@"mediatheque_back_csharp.Texts.FrTexts", Assembly.GetExecutingAssembly())
);

// Injects my search services
builder.Services.AddScoped<MySQLSimpleSearchService>();

// Injects my search managers
builder.Services.AddScoped<SimpleSearchManager>();
builder.Services.AddScoped<AdvancedSearchManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

// Custom middleware to enforce route prefix
app.UseMiddleware<GlobalRoutePrefixMiddleware>(routePrefix);
app.UsePathBase(new PathString(routePrefix));
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();