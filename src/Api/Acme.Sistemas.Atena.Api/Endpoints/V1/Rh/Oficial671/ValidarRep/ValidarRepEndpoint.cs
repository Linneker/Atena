using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.ValidarRep;

public sealed class ValidarRepEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/rh/ponto/671/validar/{empresaId:guid}", async (
            Guid empresaId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(
                new ValidarRepRequest(empresaId).ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPontoOficial, Permissions.Acoes.Ler))
        .WithTags("RH - Ponto Oficial 671")
        .WithName("ValidarRep")
        .Produces<ValidarRepResponse>();
    }
}
