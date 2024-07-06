using mediatheque_back_csharp;

// Creates a dependency injection container
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the connection to the database
builder.Services.AddDbContext<MediathequeDbContext>();

// Retrieves the configuration settings entered into the appsettings.json file
builder.Services.Configure<MySettingsModel>(builder.Configuration.GetSection("MySettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
