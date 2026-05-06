using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Divida.Command.AlterarDivida;
using Acme.Sistemas.Services.V1.Divida.Command.CriarDivida;
using Acme.Sistemas.Services.V1.Divida.Command.ExcluirDivida;
using Acme.Sistemas.Services.V1.Divida.Query.ListarDividas;
using Acme.Sistemas.Services.V1.Divida.Query.ObterDivida;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dividas;

public sealed record AlterarDividaRequest(
    string Credor, string? Descricao, decimal ValorOriginal, decimal? TaxaJurosMensal,
    DateTime DataInicio, DateTime? DataFim, int NumeroParcelas);

public sealed class DividasEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/dividas")
            .RequireAuthorization()
            .WithTags("Dividas");

        group.MapPost("/", async (CriarDividaCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/dividas/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("CriarDivida").Produces<CriarDividaCommandResult>(StatusCodes.Status201Created);

        group.MapPut("/{id:guid}", async (Guid id, AlterarDividaRequest req, IMediator m, CancellationToken ct) =>
        {
            var cmd = new AlterarDividaCommand(id, req.Credor, req.Descricao, req.ValorOriginal,
                req.TaxaJurosMensal, req.DataInicio, req.DataFim, req.NumeroParcelas);
            var r = await m.Send(cmd, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("AlterarDivida").Produces<AlterarDividaCommandResult>();

        group.MapDelete("/{id:guid}", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ExcluirDividaCommand(id), ct);
            return r.IsSuccess ? Results.NoContent() : Results.Json(r, statusCode: r.Status);
        }).WithName("ExcluirDivida").Produces(StatusCodes.Status204NoContent);

        group.MapGet("/", async (StatusConta? status, int? skip, int? take, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ListarDividasQuery(status, skip ?? 0, take ?? 50), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ListarDividas").Produces<ListarDividasQueryResult>();

        group.MapGet("/{id:guid}", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ObterDividaQuery(id), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ObterDivida").Produces<ObterDividaQueryResult>().ProducesProblem(StatusCodes.Status404NotFound);
    }
}
