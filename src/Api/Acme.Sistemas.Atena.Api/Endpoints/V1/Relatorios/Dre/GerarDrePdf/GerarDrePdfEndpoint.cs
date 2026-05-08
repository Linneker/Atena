using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Reports;
using Acme.Sistemas.Domain.Reports;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Dre.GerarDrePdf;

public sealed class GerarDrePdfEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/relatorios/financeiro/dre.pdf", async (
            [AsParameters] GerarDrePdfRequest request,
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

            var bytes = pdf.RenderDRE(result.Content, branding);
            return Results.File(bytes, "application/pdf",
                $"dre-{request.Inicio:yyyyMMdd}-{request.Fim:yyyyMMdd}.pdf");
        })
        .RequireAuthorization()
        .WithTags("RelatoriosFinanceiros")
        .WithName("GerarDREPdf")
        .Produces(StatusCodes.Status200OK, contentType: "application/pdf");
    }
}
