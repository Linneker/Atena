using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Mobile.ObterConfiguracao;

public sealed class ObterConfiguracaoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/mobile/configuracao", async (
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new ObterConfiguracaoRequest().ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Mobile")
        .WithName("ObterConfiguracaoMobile")
        .Produces<ObterConfiguracaoResponse>();
    }
}
