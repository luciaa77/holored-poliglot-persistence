using HoloRed.Infrastructure.Redis;
using HoloRed.Application.Services;
using HoloRed.Infrastructure.Cassandra;
using HoloRed.Domain.Telemetria;
using HoloRed.Infrastructure.Neo4j;
using HoloRed.Domain.Inteligencia;
using HoloRed.Infrastructure.Cassandra;
using HoloRed.Infrastructure.Neo4j;
using HoloRed.Domain.Telemetria;
using HoloRed.Domain.Inteligencia;

var builder = WebApplication.CreateBuilder(args);

// Controllers (NO Minimal API)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<RedisConnection>();
builder.Services.AddSingleton(new DockingService(numeroBahias: 5));
builder.Services.AddSingleton<CassandraSessionFactory>();
builder.Services.AddSingleton<ITelemetriaRepository, CassandraTelemetriaRepository>();
builder.Services.AddSingleton<Neo4jDriverFactory>();
builder.Services.AddSingleton<IInteligenciaRepository, Neo4jInteligenciaRepository>();
builder.Services.AddSingleton<CassandraSessionFactory>();
builder.Services.AddSingleton<ITelemetriaRepository, CassandraTelemetriaRepository>();

builder.Services.AddSingleton<Neo4jDriverFactory>();
builder.Services.AddSingleton<IInteligenciaRepository, Neo4jInteligenciaRepository>();

builder.Services.AddScoped<TelemetriaService>();
builder.Services.AddScoped<InteligenciaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();