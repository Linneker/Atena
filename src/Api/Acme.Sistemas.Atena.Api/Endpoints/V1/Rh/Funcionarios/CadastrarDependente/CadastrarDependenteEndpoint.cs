using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.CadastrarDependente;

public sealed class CadastrarDependenteEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/funcionarios/{id:guid}/dependentes", async (
            Guid id,
            CadastrarDependenteRequest body,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = body with { FuncionarioId = id };
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created(
                $"/api/v1/rh/funcionarios/{id}/dependentes/{response.Id}", response);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhDependente, Permissions.Acoes.Criar))
        .WithTags("RH - Funcionários")
        .WithName("CadastrarDependente")
        .Produces<CadastrarDependenteResponse>(StatusCodes.Status201Created)
        .ProducesProblem(404)
        .ProducesValidationProblem();
    }
}
