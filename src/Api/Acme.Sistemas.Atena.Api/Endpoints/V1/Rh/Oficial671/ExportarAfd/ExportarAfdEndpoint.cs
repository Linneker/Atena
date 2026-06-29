using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.ExportarAfd;

public sealed class ExportarAfdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/ponto/671/afd/exportar", async (
            ExportarAfdRequest req, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(req.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPontoOficial, Permissions.Acoes.ExportarAfd))
        .WithTags("RH - Ponto Oficial 671")
        .WithName("ExportarAfd")
        .Produces<ExportarAfdResponse>()
        .ProducesValidationProblem();
    }
}
