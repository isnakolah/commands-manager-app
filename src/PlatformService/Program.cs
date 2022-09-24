using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var configuration = builder.Configuration;

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>(
    client => client.BaseAddress = new Uri(configuration["CommandService:BaseUrl"]));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (env.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(
        opt => opt.UseInMemoryDatabase("InMem"));
}
else if (env.IsProduction())
{
    builder.Services.AddDbContext<AppDbContext>(
        opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI();
}

app.MapControllers();
app.PrepPopulation(env.IsProduction());

app.Run();