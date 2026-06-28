using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.AlterarFuncionarioContrato;

public sealed class AlterarFuncionarioContratoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/rh/funcionarios/{id:guid}/contrato", async (
            Guid id,
            AlterarFuncionarioContratoRequest body,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = body with { Id = id };
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhFuncionario, Permissions.Acoes.Editar))
        .WithTags("RH - Funcionários")
        .WithName("AlterarFuncionarioContrato")
        .Produces<AlterarFuncionarioContratoResponse>()
        .ProducesProblem(404)
        .ProducesValidationProblem();
    }
}
