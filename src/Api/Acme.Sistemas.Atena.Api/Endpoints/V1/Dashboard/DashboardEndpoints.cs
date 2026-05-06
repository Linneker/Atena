using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Dashboard.Query.EvolucaoFinanceira;
using Acme.Sistemas.Services.V1.Dashboard.Query.ObterKpis;
using Acme.Sistemas.Services.V1.Relatorios.Aging;
using Acme.Sistemas.Services.V1.Relatorios.PosicaoEstoque;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dashboard;

public sealed class DashboardEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var dash = app.MapGroup("/api/v1/dashboard")
            .RequireAuthorization()
            .WithTags("Dashboard");

        dash.MapGet("/kpis", async (DateTime? inicio, DateTime? fim, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ObterKpisQuery(inicio, fim), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ObterKpis").Produces<ObterKpisQueryResult>();

        dash.MapGet("/evolucao-financeira", async (int? meses, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new EvolucaoFinanceiraQuery(meses ?? 12), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("EvolucaoFinanceira").Produces<EvolucaoFinanceiraQueryResult>();

        var rel = app.MapGroup("/api/v1/relatorios")
            .RequireAuthorization()
            .WithTags("Relatorios");

        rel.MapGet("/contas-pagar/aging", async (IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new AgingQuery(TipoAging.ContasPagar), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("AgingContasPagar").Produces<AgingQueryResult>();

        rel.MapGet("/contas-receber/aging", async (IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new AgingQuery(TipoAging.ContasReceber), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("AgingContasReceber").Produces<AgingQueryResult>();

        rel.MapGet("/estoque/posicao", async (Guid? estoqueId, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new PosicaoEstoqueQuery(estoqueId), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("PosicaoEstoque").Produces<PosicaoEstoqueQueryResult>();
    }
}
