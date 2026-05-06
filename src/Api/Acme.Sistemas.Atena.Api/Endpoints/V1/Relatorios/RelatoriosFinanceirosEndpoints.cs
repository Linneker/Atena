using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.Relatorios.Financeiro.Balanco;
using Acme.Sistemas.Services.V1.Relatorios.Financeiro.DRE;
using Acme.Sistemas.Services.V1.Relatorios.Pdf;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios;

public sealed class RelatoriosFinanceirosEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/relatorios/financeiro")
            .RequireAuthorization()
            .WithTags("RelatoriosFinanceiros");

        group.MapGet("/dre", async (
            DateTime inicio, DateTime fim, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new GerarDREQuery(inicio, fim), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("GerarDRE").Produces<DREResult>();

        group.MapGet("/dre.pdf", async (
            DateTime inicio, DateTime fim,
            IMediator m,
            ITenantContext tenantContext,
            ITenantRepository tenants,
            IRelatorioPdfRenderer pdf,
            CancellationToken ct) =>
        {
            var r = await m.Send(new GerarDREQuery(inicio, fim), ct);
            if (!r.IsSuccess) return Results.Json(r, statusCode: r.Status);

            var tenant = await tenants.GetByIdAsync(tenantContext.TenantId, ct);
            var branding = new TenantBranding(
                tenant?.RazaoSocial ?? "Atena",
                tenant?.LogoUrl,
                tenant?.CorPrimaria);

            var bytes = pdf.RenderDRE(r.Content!, branding);
            return Results.File(bytes, "application/pdf",
                $"dre-{inicio:yyyyMMdd}-{fim:yyyyMMdd}.pdf");
        }).WithName("GerarDREPdf").Produces(StatusCodes.Status200OK, contentType: "application/pdf");

        group.MapGet("/balanco", async (
            DateTime dataReferencia, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new GerarBalancoQuery(dataReferencia), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("GerarBalanco").Produces<BalancoResult>();

        group.MapGet("/balanco.pdf", async (
            DateTime dataReferencia,
            IMediator m,
            ITenantContext tenantContext,
            ITenantRepository tenants,
            IRelatorioPdfRenderer pdf,
            CancellationToken ct) =>
        {
            var r = await m.Send(new GerarBalancoQuery(dataReferencia), ct);
            if (!r.IsSuccess) return Results.Json(r, statusCode: r.Status);

            var tenant = await tenants.GetByIdAsync(tenantContext.TenantId, ct);
            var branding = new TenantBranding(
                tenant?.RazaoSocial ?? "Atena",
                tenant?.LogoUrl,
                tenant?.CorPrimaria);

            var bytes = pdf.RenderBalanco(r.Content!, branding);
            return Results.File(bytes, "application/pdf",
                $"balanco-{dataReferencia:yyyyMMdd}.pdf");
        }).WithName("GerarBalancoPdf").Produces(StatusCodes.Status200OK, contentType: "application/pdf");
    }
}
