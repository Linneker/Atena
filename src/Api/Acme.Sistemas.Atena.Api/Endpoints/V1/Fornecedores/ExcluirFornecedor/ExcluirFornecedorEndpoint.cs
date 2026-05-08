using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fornecedores.ExcluirFornecedor;

public sealed class ExcluirFornecedorEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/fornecedores/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ExcluirFornecedorRequest(id);
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            return result.IsSuccess
                ? Results.NoContent()
                : Results.Json(result, statusCode: result.Status);
        })
        .RequireAuthorization()
        .WithTags("Fornecedores")
        .WithName("ExcluirFornecedor")
        .Produces(StatusCodes.Status204NoContent);
    }
}
