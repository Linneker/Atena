using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Tenant.Command.ExcluirTenant;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.ExcluirTenant;

public sealed class ExcluirTenantEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/tenants/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new ExcluirTenantCommand(id), cancellationToken);
            return response.IsSuccess
                ? Results.NoContent()
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Tenants")
        .WithName("ExcluirTenant")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
