using ApplicationCore.AutoMapper;
using Infrastructure.MySQL;
using mediatheque_back_csharp.Configuration;
using mediatheque_back_csharp.Middlewares;
using System.Reflection;
using System.Resources;

var routePrefix = "/api";

// Creates a dependency injection container
var builder = WebApplication.CreateBuilder(args);

// Injects tools for using controllers
builder.Services.AddControllers();

// Injects databases' contexts
StartUpDI.InjectDbContexts(builder);

// Injects repositories for the MySQL database
StartUpDI.InjectMySQLRepositories(builder);

// Injects my search services
StartUpDI.InjectServices(builder);

// Injects my search managers
StartUpDI.InjectManagers(builder);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(StartUpOptions.GetSwaggerGenOptions(routePrefix));

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