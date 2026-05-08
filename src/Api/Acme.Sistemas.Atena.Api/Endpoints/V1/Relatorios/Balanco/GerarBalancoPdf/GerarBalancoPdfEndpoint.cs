using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Reports;
using Acme.Sistemas.Domain.Reports;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Balanco.GerarBalancoPdf;

public sealed class GerarBalancoPdfEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/relatorios/financeiro/balanco.pdf", async (
            [AsParameters] GerarBalancoPdfRequest request,
            IMediator mediator,
            ITenantContext tenantContext,
            ITenantRepository tenants,
            IRelatorioPdfRenderer pdf,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var tenant = await tenants.GetByIdAsync(tenantContext.TenantId, cancellationToken);
            var branding = new TenantBranding(
                tenant?.RazaoSocial ?? "Atena",
                tenant?.LogoUrl,
                tenant?.CorPrimaria);

            var bytes = pdf.RenderBalanco(result.Content, branding);
            return Results.File(bytes, "application/pdf",
                $"balanco-{request.DataReferencia:yyyyMMdd}.pdf");
        })
        .RequireAuthorization()
        .WithTags("RelatoriosFinanceiros")
        .WithName("GerarBalancoPdf")
        .Produces(StatusCodes.Status200OK, contentType: "application/pdf");
    }
}
