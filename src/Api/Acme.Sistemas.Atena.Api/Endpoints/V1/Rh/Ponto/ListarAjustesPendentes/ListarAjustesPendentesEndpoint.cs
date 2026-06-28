using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ListarAjustesPendentes;

public sealed class ListarAjustesPendentesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/rh/ponto/ajustes/pendentes", async (
            int? skip,
            int? take,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ListarAjustesPendentesRequest(skip ?? 0, take ?? 50);
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.AprovarPonto))
        .WithTags("RH - Ponto")
        .WithName("ListarAjustesPendentes")
        .Produces<ListarAjustesPendentesResponse>();
    }
}
