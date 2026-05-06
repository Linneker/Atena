using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Tenant.Query.ObterTenant;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.ObterTenant;

public sealed class ObterTenantEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/tenants/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new ObterTenantQuery(id), cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Tenants")
        .WithName("ObterTenant")
        .Produces<ObterTenantQueryResult>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
