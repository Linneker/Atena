using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.ObterComprovantePdf;

public sealed class ObterComprovantePdfEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/rh/ponto/671/comprovantes/{marcacaoId:guid}.pdf", async (
            Guid marcacaoId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(
                new ObterComprovantePdfRequest(marcacaoId).ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.File(result.Content.PdfBytes, result.Content.ContentType, result.Content.FileName);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPontoOficial, Permissions.Acoes.EmitirComprovante2via))
        .WithTags("RH - Ponto Oficial 671")
        .WithName("ObterComprovantePdf")
        .Produces(200, contentType: "application/pdf")
        .ProducesProblem(404);
    }
}
