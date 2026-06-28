using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.DevolucaoVenda.RegistrarDevolucao;

public sealed class RegistrarDevolucaoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/devolucoes-venda", async (
            RegistrarDevolucaoRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/devolucoes-venda/{response.DevolucaoId}", response);
        })
        .RequireAuthorization()
        .WithTags("DevolucoesVenda")
        .WithName("RegistrarDevolucao")
        .Produces<RegistrarDevolucaoResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
