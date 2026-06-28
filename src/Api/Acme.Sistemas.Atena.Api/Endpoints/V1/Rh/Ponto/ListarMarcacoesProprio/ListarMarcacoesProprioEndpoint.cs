using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ListarMarcacoesProprio;

public sealed class ListarMarcacoesProprioEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/rh/ponto/proprio", async (
            DateOnly? dataInicio,
            DateOnly? dataFim,
            IMediator mediator,
            ITenantContext tenantContext,
            CancellationToken cancellationToken) =>
        {
            var hoje = DateOnly.FromDateTime(DateTime.UtcNow);
            var inicio = dataInicio ?? hoje.AddDays(-30);
            var fim = dataFim ?? hoje;
            var funcionarioId = tenantContext.UserId ?? Guid.Empty;

            var result = await mediator.Send(
                new ListarMarcacoesProprioRequest(inicio, fim).ToQuery(funcionarioId), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.Ler))
        .WithTags("RH - Ponto")
        .WithName("ListarMarcacoesProprio")
        .Produces<ListarMarcacoesProprioResponse>();
    }
}
