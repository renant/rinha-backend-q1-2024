using RinhaBackEnd2024;
using RinhaBackEnd2024.Models;
using RinhaBackEnd2024.Persistence.Interfaces;
using RinhaBackEnd2024.Persistence.Repositories;
using System.Text.Json;


var builder = WebApplication.CreateBuilder();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, SourceGenerationContext.Default);
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});

var connectionString = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");

builder.Services.AddNpgsqlDataSource(connectionString);
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();

var app = builder.Build();

app.MapGet("/clientes/{id}/extrato", async (int id, ITransacaoRepository _transacaoRepository) =>
{
    if (id < 1 || id > 5)
    {
        return Results.NotFound();
    }

    var extrato = await _transacaoRepository.BuscarExtrato(id);

    return Results.Ok(extrato);
});

app.MapPost("/clientes/{id}/transacoes", async (int id, TransacaoRequest req, ITransacaoRepository _transacaoRepository) =>
{
    if (id < 1 || id > 5)
    {
        return Results.NotFound();
    }

    var transacao = new Transacao
        (
            req.valor,
            req.tipo,
            req.descricao,
            id
        );

    if (!transacao.IsValid())
    {
        return Results.UnprocessableEntity();
    }

    var atualizarSaldo = await _transacaoRepository.Adicionar(transacao);

    if (atualizarSaldo.validation_error)
        return Results.UnprocessableEntity();


    return Results.Ok(atualizarSaldo);
});

app.MapPost("/reset", async (ITransacaoRepository _transacaoRepository) =>
{
    await _transacaoRepository.Reset();
});

app.Run();