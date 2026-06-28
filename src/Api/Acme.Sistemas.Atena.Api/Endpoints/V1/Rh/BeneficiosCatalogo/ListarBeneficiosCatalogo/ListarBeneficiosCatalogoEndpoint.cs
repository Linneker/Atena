using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BeneficiosCatalogo.ListarBeneficiosCatalogo;

public sealed class ListarBeneficiosCatalogoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/rh/beneficios/catalogo", async (
            int? skip,
            int? take,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ListarBeneficiosCatalogoRequest(skip ?? 0, take ?? 50);
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhBeneficio, Permissions.Acoes.Ler))
        .WithTags("RH - Benefícios")
        .WithName("ListarBeneficiosCatalogo")
        .Produces<ListarBeneficiosCatalogoResponse>();
    }
}
