using ApplicationCore.AutoMapper;
using ApplicationCore.Configuration;
using Infrastructure.MySQL;
using mediatheque_back_csharp.Managers.SearchManagers;
using mediatheque_back_csharp.Middlewares;
using System.Reflection;
using System.Resources;

var routePrefix = "/api";

// Creates a dependency injection container
var builder = WebApplication.CreateBuilder(args);

// Retrieves the configuration settings entered into Infrastructure.MySQL project
var myAppSettings = new MySQLDatabaseSettings();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(ServicesOptions.GetSwaggerGenOptions(routePrefix));

// Add the connection to the database
builder.Services.AddDbContext<MySQLDbContext>();

// Configures CORS policy
builder.Services.AddCors(ServicesOptions.GetCorsOptions(myAppSettings));

// Configures AutoMapper used for converting my POCOs into DTOs
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
});

// Injects texts resources
builder.Services.AddScoped<ResourceManager>(provider => 
    new ResourceManager(@"mediatheque_back_csharp.Texts.FrTexts", Assembly.GetExecutingAssembly())
);

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