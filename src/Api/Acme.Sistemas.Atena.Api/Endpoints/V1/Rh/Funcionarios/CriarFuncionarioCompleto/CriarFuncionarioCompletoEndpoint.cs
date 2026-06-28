using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.CriarFuncionarioCompleto;

public sealed class CriarFuncionarioCompletoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/funcionarios", async (
            CriarFuncionarioCompletoRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/rh/funcionarios/{response.FuncionarioId}", response);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhFuncionario, Permissions.Acoes.Criar))
        .WithTags("RH - Funcionários")
        .WithName("CriarFuncionarioCompleto")
        .Produces<CriarFuncionarioCompletoResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
