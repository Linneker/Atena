using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.EnviarParaAprovacao;

public sealed class EnviarParaAprovacaoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/solicitacoes-compra/{id:guid}/enviar-aprovacao", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new EnviarParaAprovacaoRequest(id);
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("SolicitacoesCompra")
        .WithName("EnviarParaAprovacao")
        .Produces<EnviarParaAprovacaoResponse>();
    }
}
