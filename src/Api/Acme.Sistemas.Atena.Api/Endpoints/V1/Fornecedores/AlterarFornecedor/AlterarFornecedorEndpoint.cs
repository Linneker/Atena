using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fornecedores.AlterarFornecedor;

public sealed class AlterarFornecedorEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/fornecedores/{id:guid}", async (
            Guid id,
            AlterarFornecedorRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(id), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Fornecedores")
        .WithName("AlterarFornecedor")
        .Produces<AlterarFornecedorResponse>()
        .ProducesValidationProblem();
    }
}
