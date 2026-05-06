using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Tenant.Query.ListarTenants;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.ListarTenants;

public sealed class ListarTenantsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/tenants", async (
            int? skip,
            int? take,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(
                new ListarTenantsQuery(skip ?? 0, take ?? 50),
                cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Tenants")
        .WithName("ListarTenants")
        .Produces<ListarTenantsQueryResult>();
    }
}
