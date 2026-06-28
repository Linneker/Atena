using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.ObterSaldo;

public sealed class ObterSaldoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/rh/banco-horas/saldo", async (
            Guid funcionarioId, string competencia, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(
                new ObterSaldoRequest(funcionarioId, competencia).ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhBancoHoras, Permissions.Acoes.Ler))
        .WithTags("RH - Banco de Horas")
        .WithName("ObterSaldo")
        .Produces<ObterSaldoResponse>();
    }
}
