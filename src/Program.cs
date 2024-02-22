global using FastEndpoints;
using Npgsql;
using RinhaBackEnd2024.Persistence.Interfaces;
using RinhaBackEnd2024.Persistence.Repositories;
using RinhaBackEnd2024.Persistence.UnitOfWork;
using System.Data;

var bld = WebApplication.CreateBuilder();
bld.Services.AddFastEndpoints();

var connectionString = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");

bld.Services.AddScoped<IDbConnection>(db => new NpgsqlConnection(connectionString));
bld.Services.AddScoped<DbSession>();
bld.Services.AddTransient<IUnitOfWork, UnitOfWork>();
bld.Services.AddScoped<IClienteRepository, ClienteRepository>();
bld.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();

var app = bld.Build();
app.UseFastEndpoints();
app.Run();