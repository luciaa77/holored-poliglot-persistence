using HoloRed.Infrastructure.Redis;
using HoloRed.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers (NO Minimal API)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<RedisConnection>();
builder.Services.AddSingleton(new DockingService(numeroBahias: 5));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();