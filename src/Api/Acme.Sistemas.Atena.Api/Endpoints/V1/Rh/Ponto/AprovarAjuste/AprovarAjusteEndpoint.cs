using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.AprovarAjuste;

public sealed class AprovarAjusteEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/ponto/ajustes/{id:guid}/aprovar", async (
            Guid id,
            AprovarAjusteRequest? body,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new AprovarAjusteRequest(id, body?.Justificativa);
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.AprovarPonto))
        .WithTags("RH - Ponto")
        .WithName("AprovarAjuste")
        .Produces<AprovarAjusteResponse>()
        .ProducesProblem(404);
    }
}
