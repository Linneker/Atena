using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Entities.Vendas;
using Acme.Sistemas.Services.V1.DevolucaoVenda.Command.RegistrarDevolucao;
using Acme.Sistemas.Services.V1.Faturamento.Command.FaturarPedido;
using Acme.Sistemas.Services.V1.Orcamento.Command.CriarOrcamento;
using Acme.Sistemas.Services.V1.Orcamento.Query.ListarOrcamentos;
using Acme.Sistemas.Services.V1.PedidoVenda.Command.ConfirmarPedidoVenda;
using Acme.Sistemas.Services.V1.PedidoVenda.Command.CriarPedidoVenda;
using Acme.Sistemas.Services.V1.Relatorios.Vendas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas;

public sealed class VendasEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        // Orçamentos
        var orc = app.MapGroup("/api/v1/orcamentos")
            .RequireAuthorization()
            .WithTags("Orcamentos");

        orc.MapPost("/", async (CriarOrcamentoCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/orcamentos/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("CriarOrcamento").Produces<CriarOrcamentoCommandResult>(StatusCodes.Status201Created);

        orc.MapGet("/", async (StatusOrcamento? status, Guid? clienteId, int? skip, int? take, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ListarOrcamentosQuery(status, clienteId, skip ?? 0, take ?? 50), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ListarOrcamentos").Produces<ListarOrcamentosQueryResult>();

        // Pedidos de venda
        var ped = app.MapGroup("/api/v1/pedidos-venda")
            .RequireAuthorization()
            .WithTags("PedidosVenda");

        ped.MapPost("/", async (CriarPedidoVendaCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/pedidos-venda/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("CriarPedidoVenda").Produces<CriarPedidoVendaCommandResult>(StatusCodes.Status201Created);

        ped.MapPost("/{id:guid}/confirmar", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ConfirmarPedidoVendaCommand(id), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ConfirmarPedidoVenda").Produces<ConfirmarPedidoVendaCommandResult>();

        // Faturamentos
        var fat = app.MapGroup("/api/v1/faturamentos")
            .RequireAuthorization()
            .WithTags("Faturamentos");

        fat.MapPost("/", async (FaturarPedidoCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/faturamentos/{r.Content!.FaturamentoId}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("FaturarPedido").Produces<FaturarPedidoCommandResult>(StatusCodes.Status201Created);

        // Devoluções
        var dev = app.MapGroup("/api/v1/devolucoes-venda")
            .RequireAuthorization()
            .WithTags("DevolucoesVenda");

        dev.MapPost("/", async (RegistrarDevolucaoCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/devolucoes-venda/{r.Content!.DevolucaoId}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("RegistrarDevolucao").Produces<RegistrarDevolucaoCommandResult>(StatusCodes.Status201Created);

        // Relatórios
        var rel = app.MapGroup("/api/v1/relatorios/vendas")
            .RequireAuthorization()
            .WithTags("RelatoriosVendas");

        rel.MapGet("/", async (DateTime inicio, DateTime fim, AgrupamentoVendas agrupamento,
            IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new RelatorioVendasQuery(inicio, fim, agrupamento), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("RelatorioVendas").Produces<RelatorioVendasResult>();
    }
}
