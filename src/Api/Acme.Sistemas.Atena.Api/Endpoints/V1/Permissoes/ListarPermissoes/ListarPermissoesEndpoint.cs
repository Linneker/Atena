using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Permissoes.ListarPermissoes;

public sealed class ListarPermissoesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/permissoes", async (
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ListarPermissoesRequest();
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Permissao, Permissions.Acoes.Ler))
        .WithTags("Permissoes")
        .WithName("ListarPermissoes")
        .Produces<ListarPermissoesResponse>();
    }
}
