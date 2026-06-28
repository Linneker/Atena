using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.Rh.Ponto.Engine;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ObterEspelhoPdf;

public sealed class ObterEspelhoPdfEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/rh/ponto/espelho.pdf", async (
            Guid funcionarioId,
            string competencia,
            IMediator mediator,
            IGeradorEspelhoPdf gerador,
            ITenantRepository tenantRepo,
            ITenantContext tenantContext,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(
                new ObterEspelhoPdfRequest(funcionarioId, competencia).ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var tenant = await tenantRepo.GetByIdAsync(tenantContext.TenantId, cancellationToken);
            var razao = tenant?.RazaoSocial ?? "Atena ERP";

            var pdf = gerador.Gerar(result.Content, razao);
            var fileName = $"espelho-{funcionarioId:N}-{competencia}.pdf";
            return Results.File(pdf, "application/pdf", fileName);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.Ler))
        .WithTags("RH - Ponto")
        .WithName("ObterEspelhoPdf")
        .Produces(StatusCodes.Status200OK, contentType: "application/pdf")
        .ProducesProblem(404);
    }
}
