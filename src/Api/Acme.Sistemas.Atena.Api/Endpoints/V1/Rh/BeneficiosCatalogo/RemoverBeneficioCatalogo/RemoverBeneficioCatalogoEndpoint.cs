using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BeneficiosCatalogo.RemoverBeneficioCatalogo;

public sealed class RemoverBeneficioCatalogoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/rh/beneficios/catalogo/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new RemoverBeneficioCatalogoRequest(id).ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhBeneficio, Permissions.Acoes.Excluir))
        .WithTags("RH - Benefícios")
        .WithName("RemoverBeneficioCatalogo")
        .Produces<RemoverBeneficioCatalogoResponse>()
        .ProducesProblem(404);
    }
}
