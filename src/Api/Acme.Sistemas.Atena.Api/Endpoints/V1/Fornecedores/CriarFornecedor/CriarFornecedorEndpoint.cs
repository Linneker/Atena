using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fornecedores.CriarFornecedor;

public sealed class CriarFornecedorEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/fornecedores", async (
            CriarFornecedorRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/fornecedores/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("Fornecedores")
        .WithName("CriarFornecedor")
        .Produces<CriarFornecedorResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
