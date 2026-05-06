using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.ContaPagar.Command.BaixarContaPagar;
using Acme.Sistemas.Services.V1.ContaPagar.Command.CriarContaPagar;
using Acme.Sistemas.Services.V1.ContaPagar.Query.ListarContasPagar;
using Acme.Sistemas.Services.V1.ContaPagar.Query.ObterContaPagar;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasPagar;

public sealed record BaixarContaPagarRequest(
    decimal ValorPago, DateTime DataPagamento,
    FormaPagamento FormaPagamento, string? Observacao);

public sealed class ContasPagarEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/contas-pagar")
            .RequireAuthorization()
            .WithTags("ContasPagar");

        group.MapPost("/", async (CriarContaPagarCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/contas-pagar/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("CriarContaPagar").Produces<CriarContaPagarCommandResult>(StatusCodes.Status201Created);

        group.MapPost("/{id:guid}/baixar", async (Guid id, BaixarContaPagarRequest req, IMediator m, CancellationToken ct) =>
        {
            var cmd = new BaixarContaPagarCommand(id, req.ValorPago, req.DataPagamento, req.FormaPagamento, req.Observacao);
            var r = await m.Send(cmd, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("BaixarContaPagar").Produces<BaixarContaPagarCommandResult>();

        group.MapGet("/", async (
            StatusConta? status, DateTime? vencimentoInicio, DateTime? vencimentoFim,
            Guid? fornecedorId, bool? vencendoEmAteSeteDias, int? skip, int? take,
            IMediator m, CancellationToken ct) =>
        {
            var q = new ListarContasPagarQuery(status, vencimentoInicio, vencimentoFim,
                fornecedorId, vencendoEmAteSeteDias ?? false, skip ?? 0, take ?? 50);
            var r = await m.Send(q, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ListarContasPagar").Produces<ListarContasPagarQueryResult>();

        group.MapGet("/{id:guid}", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ObterContaPagarQuery(id), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ObterContaPagar").Produces<ObterContaPagarQueryResult>().ProducesProblem(404);
    }
}
