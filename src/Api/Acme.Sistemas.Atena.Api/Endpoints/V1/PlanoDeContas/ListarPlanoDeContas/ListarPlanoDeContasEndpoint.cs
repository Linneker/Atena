using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.PlanoDeContas.ListarPlanoDeContas;

public sealed class ListarPlanoDeContasEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/plano-de-contas", async (
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ListarPlanoDeContasRequest();
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("PlanoDeContas")
        .WithName("ListarPlanoDeContas")
        .Produces<ListarPlanoDeContasResponse>();
    }
}
