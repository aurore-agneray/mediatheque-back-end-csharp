using mediatheque_back_csharp;
using mediatheque_back_csharp.AutoMapper;
using mediatheque_back_csharp.Database;
using mediatheque_back_csharp.Managers.SearchManagers;
using mediatheque_back_csharp.Middlewares;
using Microsoft.OpenApi.Models;

var routePrefix = "/api";

// Creates a dependency injection container
var builder = WebApplication.CreateBuilder(args);

// Retrieves the configuration settings entered into the appsettings.json file
builder.Services.Configure<MySettingsModel>(builder.Configuration.GetSection("MySettings"));
var myAppSettings = builder.Configuration.GetSection("MySettings").Get<MySettingsModel>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Médiathèque", Version = "v1" });
    c.AddServer(new OpenApiServer
    {
        Url = routePrefix,
        Description = "Base path for the API"
    });
});

// Add the connection to the database
builder.Services.AddDbContext<MediathequeDbContext>();

// Configures CORS policy
var frontDomainsStr = myAppSettings?.FrontEndDomains;
string[] frontDomains = null;

if (frontDomainsStr != null)
{
    frontDomains = frontDomainsStr.Split(';');
}

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins(frontDomains ?? ["http://localhost:5173"])
                  .WithHeaders("Content-type")
                  .WithMethods("GET", "POST");
        });
});

// Configures AutoMapper used for converting my POCOs into DTOs
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
});

// Injects my simple search manager
builder.Services.AddScoped<SimpleSearchManager>();

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