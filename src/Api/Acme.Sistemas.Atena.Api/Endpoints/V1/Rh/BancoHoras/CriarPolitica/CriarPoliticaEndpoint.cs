using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.CriarPolitica;

public sealed class CriarPoliticaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/banco-horas/politicas", async (
            CriarPoliticaRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Created($"/api/v1/rh/banco-horas/politicas/{result.Content.Id}", result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPoliticasPonto, Permissions.Acoes.Criar))
        .WithTags("RH - Banco de Horas")
        .WithName("CriarPolitica")
        .Produces<CriarPoliticaResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
