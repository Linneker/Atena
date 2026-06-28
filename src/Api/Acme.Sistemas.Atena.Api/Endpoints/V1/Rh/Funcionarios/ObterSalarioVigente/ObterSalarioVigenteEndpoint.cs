using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.ObterSalarioVigente;

public sealed class ObterSalarioVigenteEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/rh/funcionarios/{id:guid}/salario-vigente", async (
            Guid id,
            DateOnly? em,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ObterSalarioVigenteRequest(id, em ?? DateOnly.FromDateTime(DateTime.UtcNow));
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhFuncionario, Permissions.Acoes.Ler))
        .WithTags("RH - Funcionários")
        .WithName("ObterSalarioVigente")
        .Produces<ObterSalarioVigenteResponse>();
    }
}
