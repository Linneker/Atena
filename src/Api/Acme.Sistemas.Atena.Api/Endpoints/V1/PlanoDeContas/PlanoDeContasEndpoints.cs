using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.PlanoDeContas.Command.AlterarPlanoDeContas;
using Acme.Sistemas.Services.V1.PlanoDeContas.Command.CriarPlanoDeContas;
using Acme.Sistemas.Services.V1.PlanoDeContas.Command.ExcluirPlanoDeContas;
using Acme.Sistemas.Services.V1.PlanoDeContas.Query.ListarPlanoDeContas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.PlanoDeContas;

public sealed record AlterarPlanoDeContasRequest(string Nome, bool AceitaLancamento, bool Ativo);

public sealed class PlanoDeContasEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/plano-de-contas")
            .RequireAuthorization()
            .WithTags("PlanoDeContas");

        group.MapPost("/", async (CriarPlanoDeContasCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/plano-de-contas/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("CriarPlanoDeContas").Produces<CriarPlanoDeContasCommandResult>(StatusCodes.Status201Created);

        group.MapPut("/{id:guid}", async (Guid id, AlterarPlanoDeContasRequest req, IMediator m, CancellationToken ct) =>
        {
            var cmd = new AlterarPlanoDeContasCommand(id, req.Nome, req.AceitaLancamento, req.Ativo);
            var r = await m.Send(cmd, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("AlterarPlanoDeContas").Produces<AlterarPlanoDeContasCommandResult>();

        group.MapDelete("/{id:guid}", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ExcluirPlanoDeContasCommand(id), ct);
            return r.IsSuccess ? Results.NoContent() : Results.Json(r, statusCode: r.Status);
        }).WithName("ExcluirPlanoDeContas").Produces(StatusCodes.Status204NoContent);

        group.MapGet("/", async (IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ListarPlanoDeContasQuery(), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ListarPlanoDeContas").Produces<ListarPlanoDeContasQueryResult>();
    }
}
