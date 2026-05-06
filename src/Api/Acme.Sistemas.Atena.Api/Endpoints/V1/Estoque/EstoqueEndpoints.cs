using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Entities.Estoque;
using Acme.Sistemas.Services.V1.Estoque.Command.RegistrarEntrada;
using Acme.Sistemas.Services.V1.Estoque.Command.RegistrarSaida;
using Acme.Sistemas.Services.V1.Estoque.Query.ConsultarSaldo;
using Acme.Sistemas.Services.V1.Estoque.Query.RelatorioMovimentacao;
using Acme.Sistemas.Services.V1.Inventario.Command.AbrirInventario;
using Acme.Sistemas.Services.V1.Inventario.Command.FecharInventario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque;

public sealed record RegistrarEntradaRequest(
    Guid EstoqueId, Guid ProdutoId, decimal Quantidade,
    decimal? CustoUnitario, OrigemMovimento Origem, string? Motivo,
    Guid? FornecedorId, string? DocumentoReferencia, DateTime? DataMovimento);

public sealed record RegistrarSaidaRequest(
    Guid EstoqueId, Guid ProdutoId, decimal Quantidade,
    decimal? CustoUnitario, OrigemMovimento Origem, string? Motivo,
    Guid? ClienteId, string? DocumentoReferencia, DateTime? DataMovimento);

public sealed record AbrirInventarioRequest(Guid EstoqueId, string? Observacao);

public sealed class EstoqueEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/estoque")
            .RequireAuthorization()
            .WithTags("Estoque");

        group.MapGet("/produtos/{produtoId:guid}/saldo", async (
            Guid produtoId, Guid? estoqueId, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ConsultarSaldoQuery(produtoId, estoqueId), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ConsultarSaldo").Produces<ConsultarSaldoQueryResult>();

        group.MapPost("/entradas", async (RegistrarEntradaRequest req, IMediator m, CancellationToken ct) =>
        {
            var cmd = new RegistrarEntradaCommand(
                req.EstoqueId, req.ProdutoId, req.Quantidade,
                req.CustoUnitario, req.Origem, req.Motivo,
                req.FornecedorId, req.DocumentoReferencia, req.DataMovimento);
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/estoque/movimentos/{r.Content!.MovimentoId}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("RegistrarEntrada").Produces<RegistrarEntradaCommandResult>(StatusCodes.Status201Created);

        group.MapPost("/saidas", async (RegistrarSaidaRequest req, IMediator m, CancellationToken ct) =>
        {
            var cmd = new RegistrarSaidaCommand(
                req.EstoqueId, req.ProdutoId, req.Quantidade,
                req.CustoUnitario, req.Origem, req.Motivo,
                req.ClienteId, req.DocumentoReferencia, req.DataMovimento);
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/estoque/movimentos/{r.Content!.MovimentoId}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("RegistrarSaida").Produces<RegistrarSaidaCommandResult>(StatusCodes.Status201Created);

        group.MapGet("/produtos/{produtoId:guid}/movimentacao", async (
            Guid produtoId, DateTime? inicio, DateTime? fim, int? skip, int? take,
            IMediator m, CancellationToken ct) =>
        {
            var q = new RelatorioMovimentacaoQuery(produtoId, inicio, fim, skip ?? 0, take ?? 200);
            var r = await m.Send(q, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("RelatorioMovimentacao").Produces<RelatorioMovimentacaoResult>();

        var inv = app.MapGroup("/api/v1/inventarios")
            .RequireAuthorization()
            .WithTags("Inventarios");

        inv.MapPost("/", async (AbrirInventarioRequest req, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new AbrirInventarioCommand(req.EstoqueId, req.Observacao), ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/inventarios/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("AbrirInventario").Produces<AbrirInventarioCommandResult>(StatusCodes.Status201Created);

        inv.MapPost("/{id:guid}/fechar", async (
            Guid id, FecharInventarioRequestBody body, IMediator m, CancellationToken ct) =>
        {
            var cmd = new FecharInventarioCommand(id, body.Contagens);
            var r = await m.Send(cmd, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("FecharInventario").Produces<FecharInventarioCommandResult>();
    }
}

public sealed record FecharInventarioRequestBody(IReadOnlyList<InventarioContagem> Contagens);
