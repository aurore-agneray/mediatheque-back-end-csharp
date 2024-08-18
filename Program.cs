using mediatheque_back_csharp.AutoMapper;
using mediatheque_back_csharp.Configuration;
using mediatheque_back_csharp.Database;
using mediatheque_back_csharp.Managers.SearchManagers;
using mediatheque_back_csharp.Middlewares;

const string ROUTE_PREFIX = "/api";
const string MY_SETTINGS = "MySettings";

// Creates a dependency injection container
var builder = WebApplication.CreateBuilder(args);

// Retrieves the configuration settings entered into the appsettings.json file
var myAppSettings = builder.Configuration.GetSection(MY_SETTINGS);
builder.Services.Configure<MySettingsModel>(myAppSettings);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(ServicesOptions.GetSwaggerGenOptions(ROUTE_PREFIX));

// Add the connection to the database
builder.Services.AddDbContext<MediathequeDbContext>();

// Configures CORS policy
builder.Services.AddCors(
    ServicesOptions.GetCorsOptions(myAppSettings.Get<MySettingsModel>())
);

// Configures AutoMapper used for converting my POCOs into DTOs
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
});

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
app.UseMiddleware<GlobalRoutePrefixMiddleware>(ROUTE_PREFIX);
app.UsePathBase(new PathString(ROUTE_PREFIX));
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();