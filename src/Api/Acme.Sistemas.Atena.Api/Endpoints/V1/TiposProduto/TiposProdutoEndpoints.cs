using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.TipoProduto.Command.CriarTipoProduto;
using Acme.Sistemas.Services.V1.TipoProduto.Query.ListarTiposProduto;
using Acme.Sistemas.Services.V1.TipoValorProduto.Command.CriarTipoValorProduto;
using Acme.Sistemas.Services.V1.TipoValorProduto.Query.ListarTiposValorProduto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.TiposProduto;

public sealed class TiposProdutoEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var tipos = app.MapGroup("/api/v1/tipos-produto")
            .RequireAuthorization()
            .WithTags("TiposProduto");

        tipos.MapPost("/", async (CriarTipoProdutoCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/tipos-produto/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("CriarTipoProduto").Produces<CriarTipoProdutoCommandResult>(StatusCodes.Status201Created);

        tipos.MapGet("/", async (IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ListarTiposProdutoQuery(), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ListarTiposProduto").Produces<ListarTiposProdutoQueryResult>();

        var valores = app.MapGroup("/api/v1/tipos-valor-produto")
            .RequireAuthorization()
            .WithTags("TiposValorProduto");

        valores.MapPost("/", async (CriarTipoValorProdutoCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/tipos-valor-produto/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("CriarTipoValorProduto").Produces<CriarTipoValorProdutoCommandResult>(StatusCodes.Status201Created);

        valores.MapGet("/", async (IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ListarTiposValorProdutoQuery(), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ListarTiposValorProduto").Produces<ListarTiposValorProdutoQueryResult>();
    }
}
