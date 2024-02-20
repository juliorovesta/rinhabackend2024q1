using System.Text.Json;
using Common.Http;
using Common.Serialization;
using Endpoints;
using Persistence;

var builder = WebApplication.CreateSlimBuilder(args);

if (builder.Environment.IsProduction())
    builder.Logging.ClearProviders();

builder
    .Services.AddNpgsqlSlimDataSource(builder.Configuration.GetConnectionString("Db")!)
    .AddTransient<DbContext>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.UseStatusCode422ForBadRequest();

var clientesApi = app.MapGroup("/clientes");
clientesApi.MapGet("/{id}/extrato", ExtratoEndpoint.Handle);
clientesApi.MapPost("/{id}/transacoes", TransacoesEndpoint.Handle);

app.Run();
