using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.ContaReceber.Command.CriarContaReceber;
using Acme.Sistemas.Services.V1.ContaReceber.Command.ReceberContaReceber;
using Acme.Sistemas.Services.V1.ContaReceber.Query.ListarContasReceber;
using Acme.Sistemas.Services.V1.ContaReceber.Query.ObterContaReceber;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasReceber;

public sealed record ReceberContaReceberRequest(
    decimal ValorRecebido, DateTime DataRecebimento, string? Observacao);

public sealed class ContasReceberEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/contas-receber")
            .RequireAuthorization()
            .WithTags("ContasReceber");

        group.MapPost("/", async (CriarContaReceberCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/contas-receber/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("CriarContaReceber").Produces<CriarContaReceberCommandResult>(StatusCodes.Status201Created);

        group.MapPost("/{id:guid}/receber", async (Guid id, ReceberContaReceberRequest req, IMediator m, CancellationToken ct) =>
        {
            var cmd = new ReceberContaReceberCommand(id, req.ValorRecebido, req.DataRecebimento, req.Observacao);
            var r = await m.Send(cmd, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ReceberContaReceber").Produces<ReceberContaReceberCommandResult>();

        group.MapGet("/", async (
            StatusConta? status, DateTime? vencimentoInicio, DateTime? vencimentoFim,
            Guid? clienteId, int? diasAtrasoMinimo, int? skip, int? take,
            IMediator m, CancellationToken ct) =>
        {
            var q = new ListarContasReceberQuery(status, vencimentoInicio, vencimentoFim,
                clienteId, diasAtrasoMinimo, skip ?? 0, take ?? 50);
            var r = await m.Send(q, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ListarContasReceber").Produces<ListarContasReceberQueryResult>();

        group.MapGet("/{id:guid}", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ObterContaReceberQuery(id), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ObterContaReceber").Produces<ObterContaReceberQueryResult>().ProducesProblem(404);
    }
}
