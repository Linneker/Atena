using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.PagarSaldo;

public sealed class PagarSaldoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/banco-horas/pagar", async (
            PagarSaldoRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Created($"/api/v1/rh/banco-horas/movimentos/{result.Content.MovimentoId}",
                result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhBancoHoras, Permissions.Acoes.Editar))
        .WithTags("RH - Banco de Horas")
        .WithName("PagarSaldo")
        .Produces<PagarSaldoResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
