using ApplicationCore.AutoMapper;
using MediathequeBackCSharp.Classes;
using MediathequeBackCSharp.Configuration;
using MediathequeBackCSharp.Middlewares;
using MediathequeBackCSharp.Texts;
using Microsoft.AspNetCore.RateLimiting;

var routePrefix = "/api";

// Get the version number of the assembly (currently used for the Swagger)
string? assemblyVersion = AssemblyInfo.GetVersionNumber();

// Creates a dependency injection container
var builder = WebApplication.CreateBuilder(args);

// Injects texts resources
builder.Services.AddSingleton(provider => TextsManager.Instance);

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

// Configures CORS policy for the API
builder.Services.AddCors(
    StartUpOptions.GetCorsOptions(
        builder.Configuration.GetValue<string>("FrontEndDomains")!
    )
);

// Configures API calls limits
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixedLimiter", opt =>
    {
        opt.PermitLimit = 10;
        opt.Window = TimeSpan.FromSeconds(30);
        opt.QueueLimit = 0;
    });

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

        // This option is mandatory if we want the response to return an object instead of a string !
        context.HttpContext.Response.ContentType = "application/json";

        var message = "veuillez attendre 30 secondes avant de lancer une nouvelle recherche";
        var response = new { Message = message };

        await context.HttpContext.Response.WriteAsJsonAsync(
            response,
            cancellationToken
        );
    };
});

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

// MUST BE PLACED AFTER UseRouting in order to work properly ! https://github.com/dotnet/aspnetcore/issues/45302
app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();