using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.ListarPoliticas;

public sealed class ListarPoliticasEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/rh/banco-horas/politicas", async (
            int? skip, int? take, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(
                new ListarPoliticasRequest(skip ?? 0, take ?? 50).ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPoliticasPonto, Permissions.Acoes.Ler))
        .WithTags("RH - Banco de Horas")
        .WithName("ListarPoliticas")
        .Produces<ListarPoliticasResponse>();
    }
}
