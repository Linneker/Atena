using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Produto.Command.AlterarProduto;
using Acme.Sistemas.Services.V1.Produto.Command.CriarProduto;
using Acme.Sistemas.Services.V1.Produto.Command.DefinirPreco;
using Acme.Sistemas.Services.V1.Produto.Command.ExcluirProduto;
using Acme.Sistemas.Services.V1.Produto.Query.ListarProdutos;
using Acme.Sistemas.Services.V1.Produto.Query.ObterProduto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos;

public sealed record AlterarProdutoRequest(
    string Nome, string? Descricao, string? CodigoBarras, string UnidadeMedida,
    Guid? TipoProdutoId, Guid? FornecedorId,
    decimal? CustoMedio, decimal? EstoqueMinimo, StatusAtivo Status);

public sealed record DefinirPrecoRequest(Guid TipoValorProdutoId, decimal Valor, DateTime? VigenciaInicio);

public sealed class ProdutosEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/produtos")
            .RequireAuthorization()
            .WithTags("Produtos");

        group.MapPost("/", async (CriarProdutoCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/produtos/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("CriarProduto").Produces<CriarProdutoCommandResult>(StatusCodes.Status201Created);

        group.MapPut("/{id:guid}", async (Guid id, AlterarProdutoRequest req, IMediator m, CancellationToken ct) =>
        {
            var cmd = new AlterarProdutoCommand(id, req.Nome, req.Descricao, req.CodigoBarras,
                req.UnidadeMedida, req.TipoProdutoId, req.FornecedorId,
                req.CustoMedio, req.EstoqueMinimo, req.Status);
            var r = await m.Send(cmd, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("AlterarProduto").Produces<AlterarProdutoCommandResult>();

        group.MapDelete("/{id:guid}", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ExcluirProdutoCommand(id), ct);
            return r.IsSuccess ? Results.NoContent() : Results.Json(r, statusCode: r.Status);
        }).WithName("ExcluirProduto").Produces(StatusCodes.Status204NoContent);

        group.MapPost("/{id:guid}/precos", async (Guid id, DefinirPrecoRequest req, IMediator m, CancellationToken ct) =>
        {
            var cmd = new DefinirPrecoProdutoCommand(id, req.TipoValorProdutoId, req.Valor, req.VigenciaInicio);
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/produtos/{id}/precos/{r.Content!.PrecoId}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("DefinirPrecoProduto").Produces<DefinirPrecoProdutoCommandResult>(StatusCodes.Status201Created);

        group.MapGet("/", async (string? termo, int? skip, int? take, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ListarProdutosQuery(termo, skip ?? 0, take ?? 50), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ListarProdutos").Produces<ListarProdutosQueryResult>();

        group.MapGet("/{id:guid}", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ObterProdutoQuery(id), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ObterProduto").Produces<ObterProdutoQueryResult>().ProducesProblem(404);
    }
}
