using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.BaterPonto;

public sealed class BaterPontoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/ponto/bater", async (
            BaterPontoRequest request,
            IMediator mediator,
            HttpContext http,
            CancellationToken cancellationToken) =>
        {
            var ip = http.Connection.RemoteIpAddress?.ToString();
            var ua = http.Request.Headers.UserAgent.ToString();
            var deviceId = http.Request.Headers["X-Device-Id"].ToString();
            var result = await mediator.Send(request.ToCommand(ip, ua, deviceId), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/rh/ponto/proprio/{response.Id}", response);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.BaterPonto))
        .WithTags("RH - Ponto")
        .WithName("BaterPonto")
        .Produces<BaterPontoResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
