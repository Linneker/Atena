using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ListarMarcacoesEquipe;

public sealed class ListarMarcacoesEquipeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/rh/ponto/equipe/{funcionarioId:guid}", async (
            Guid funcionarioId,
            DateOnly? dataInicio,
            DateOnly? dataFim,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var hoje = DateOnly.FromDateTime(DateTime.UtcNow);
            var inicio = dataInicio ?? hoje.AddDays(-30);
            var fim = dataFim ?? hoje;

            var result = await mediator.Send(
                new ListarMarcacoesEquipeRequest(funcionarioId, inicio, fim).ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.GerirEquipe))
        .WithTags("RH - Ponto")
        .WithName("ListarMarcacoesEquipe")
        .Produces<ListarMarcacoesEquipeResponse>();
    }
}
