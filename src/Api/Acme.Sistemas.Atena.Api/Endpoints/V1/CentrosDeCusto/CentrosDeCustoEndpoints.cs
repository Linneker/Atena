using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.CentroDeCusto.Command.AlterarCentroDeCusto;
using Acme.Sistemas.Services.V1.CentroDeCusto.Command.CriarCentroDeCusto;
using Acme.Sistemas.Services.V1.CentroDeCusto.Command.ExcluirCentroDeCusto;
using Acme.Sistemas.Services.V1.CentroDeCusto.Query.ListarCentrosDeCusto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.CentrosDeCusto;

public sealed record AlterarCentroDeCustoRequest(
    string Nome, string? Descricao, Guid? ResponsavelId, bool Ativo);

public sealed class CentrosDeCustoEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/centros-de-custo")
            .RequireAuthorization()
            .WithTags("CentrosDeCusto");

        group.MapPost("/", async (CriarCentroDeCustoCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/centros-de-custo/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("CriarCentroDeCusto").Produces<CriarCentroDeCustoCommandResult>(StatusCodes.Status201Created);

        group.MapPut("/{id:guid}", async (Guid id, AlterarCentroDeCustoRequest req, IMediator m, CancellationToken ct) =>
        {
            var cmd = new AlterarCentroDeCustoCommand(id, req.Nome, req.Descricao, req.ResponsavelId, req.Ativo);
            var r = await m.Send(cmd, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("AlterarCentroDeCusto").Produces<AlterarCentroDeCustoCommandResult>();

        group.MapDelete("/{id:guid}", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ExcluirCentroDeCustoCommand(id), ct);
            return r.IsSuccess ? Results.NoContent() : Results.Json(r, statusCode: r.Status);
        }).WithName("ExcluirCentroDeCusto").Produces(StatusCodes.Status204NoContent);

        group.MapGet("/", async (int? skip, int? take, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ListarCentrosDeCustoQuery(skip ?? 0, take ?? 100), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ListarCentrosDeCusto").Produces<ListarCentrosDeCustoQueryResult>();
    }
}
