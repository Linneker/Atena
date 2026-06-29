using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.SalvarConfiguracaoRep;

public sealed class SalvarConfiguracaoRepEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/ponto/671/configuracao", async (
            SalvarConfiguracaoRepRequest req,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(req.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPontoOficial, Permissions.Acoes.ConfigurarRep))
        .WithTags("RH - Ponto Oficial 671")
        .WithName("SalvarConfiguracaoRep")
        .Produces<SalvarConfiguracaoRepResponse>()
        .ProducesValidationProblem();
    }
}
