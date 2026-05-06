using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Entities.Compras;
using Acme.Sistemas.Services.V1.PedidoCompra.Command.CriarPedidoCompra;
using Acme.Sistemas.Services.V1.PedidoCompra.Command.EnviarFornecedor;
using Acme.Sistemas.Services.V1.RecebimentoCompra.Command.RegistrarRecebimento;
using Acme.Sistemas.Services.V1.RecebimentoCompra.Command.VincularNFe;
using Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.AprovarSolicitacao;
using Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.CriarSolicitacao;
using Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.EnviarParaAprovacao;
using Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.RejeitarSolicitacao;
using Acme.Sistemas.Services.V1.SolicitacaoCompra.Query.ListarSolicitacoes;
using Acme.Sistemas.Services.V1.SolicitacaoCompra.Query.ObterSolicitacao;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras;

public sealed record EnviarFornecedorRequest(string? EmailDestinoOverride);
public sealed record RejeitarSolicitacaoRequest(string Motivo);
public sealed record VincularNFeRequest(string NumeroNotaFiscal, string ChaveAcesso);

public sealed class ComprasEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        // ---- Solicitações de compra
        var sols = app.MapGroup("/api/v1/solicitacoes-compra")
            .RequireAuthorization()
            .WithTags("SolicitacoesCompra");

        sols.MapPost("/", async (CriarSolicitacaoCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/solicitacoes-compra/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("CriarSolicitacaoCompra").Produces<CriarSolicitacaoCommandResult>(StatusCodes.Status201Created);

        sols.MapGet("/", async (StatusSolicitacaoCompra? status, int? skip, int? take, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ListarSolicitacoesQuery(status, skip ?? 0, take ?? 50), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ListarSolicitacoes").Produces<ListarSolicitacoesQueryResult>();

        sols.MapGet("/{id:guid}", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ObterSolicitacaoQuery(id), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ObterSolicitacao").Produces<ObterSolicitacaoQueryResult>().ProducesProblem(404);

        sols.MapPost("/{id:guid}/enviar-aprovacao", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new EnviarParaAprovacaoCommand(id), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("EnviarParaAprovacao").Produces<EnviarParaAprovacaoCommandResult>();

        sols.MapPost("/{id:guid}/aprovar", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new AprovarSolicitacaoCommand(id), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("AprovarSolicitacao").Produces<AprovarSolicitacaoCommandResult>();

        sols.MapPost("/{id:guid}/rejeitar", async (Guid id, RejeitarSolicitacaoRequest req, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new RejeitarSolicitacaoCommand(id, req.Motivo), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("RejeitarSolicitacao").Produces<RejeitarSolicitacaoCommandResult>();

        // ---- Pedidos de compra
        var peds = app.MapGroup("/api/v1/pedidos-compra")
            .RequireAuthorization()
            .WithTags("PedidosCompra");

        peds.MapPost("/", async (CriarPedidoCompraCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/pedidos-compra/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("CriarPedidoCompra").Produces<CriarPedidoCompraCommandResult>(StatusCodes.Status201Created);

        peds.MapPost("/{id:guid}/enviar-fornecedor", async (Guid id, EnviarFornecedorRequest req, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new EnviarFornecedorCommand(id, req.EmailDestinoOverride), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("EnviarFornecedor").Produces<EnviarFornecedorCommandResult>();

        // ---- Recebimentos
        var rec = app.MapGroup("/api/v1/recebimentos-compra")
            .RequireAuthorization()
            .WithTags("RecebimentosCompra");

        rec.MapPost("/", async (RegistrarRecebimentoCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/recebimentos-compra/{r.Content!.RecebimentoId}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("RegistrarRecebimento").Produces<RegistrarRecebimentoCommandResult>(StatusCodes.Status201Created);

        rec.MapPost("/{id:guid}/vincular-nfe", async (Guid id, VincularNFeRequest req, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new VincularNFeCommand(id, req.NumeroNotaFiscal, req.ChaveAcesso), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("VincularNFeRecebimento").Produces<VincularNFeCommandResult>();
    }
}
