using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Tenant.Command.AlterarTenant;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.AlterarTenant;

public sealed class AlterarTenantEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/tenants/{id:guid}", async (
            Guid id,
            AlterarTenantRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new AlterarTenantCommand(
                id, request.RazaoSocial, request.Plano, request.Status,
                request.LogoUrl, request.CorPrimaria, request.FusoHorario);
            var response = await mediator.Send(command, cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Tenants")
        .WithName("AlterarTenant")
        .Produces<AlterarTenantCommandResult>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
