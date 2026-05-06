using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.RegistrarTenant;

public sealed class RegistrarTenantEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/tenants/registrar", async (
            RegistrarTenantRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(request.ToCommand(), cancellationToken);
            return response.IsSuccess
                ? Results.Created($"/api/v1/tenants/{response.Content!.Id}", response.Content.ToResponse())
                : Results.Json(response, statusCode: response.Status);
        })
        .AllowAnonymous()
        .RequireRateLimiting("tenant-registration")
        .WithTags("Tenants")
        .WithName("RegistrarTenant")
        .Produces<RegistrarTenantResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
