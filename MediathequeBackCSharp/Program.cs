using ApplicationCore.AutoMapper;
using Infrastructure.MySQL;
using MediathequeBackCSharp.Classes;
using MediathequeBackCSharp.Configuration;
using MediathequeBackCSharp.Middlewares;
using System.Reflection;
using System.Resources;

var routePrefix = "/api";

// Get the version number of the assembly (currently used for the Swagger)
string? assemblyVersion = AssemblyInfo.GetVersionNumber();

// Creates a dependency injection container
var builder = WebApplication.CreateBuilder(args);

// Injects texts resources
builder.Services.AddScoped<ResourceManager>(provider =>
    new ResourceManager(@"MediathequeBackCSharp.Texts.FrTexts", Assembly.GetExecutingAssembly())
);

// Store the TextManager into a variable that can be injected into StartUp static methods
//var serviceProvider = builder.Services.BuildServiceProvider();
//var textManager = serviceProvider.GetService<ResourceManager>();

// Injects tools for using controllers
builder.Services.AddControllers();

// Injects databases' contexts
StartUpDI.InjectDbContexts(builder);

// Injects repositories for any source of data
StartUpDI.InjectRepositories(builder);

// Injects my search services
StartUpDI.InjectServices(builder);

// Injects my search managers
StartUpDI.InjectManagers(builder);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(StartUpOptions.GetSwaggerGenOptions(assemblyVersion, routePrefix));

// Retrieves the configuration settings entered into Infrastructure.MySQL project
var mySQLSettings = new MySQLDatabaseSettings();

// Configures CORS policy
builder.Services.AddCors(StartUpOptions.GetCorsOptions(mySQLSettings));

// Configures AutoMapper used for converting my POCOs into DTOs
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"/swagger/{assemblyVersion}/swagger.json", $"{assemblyVersion}");
    });
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