using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Funcionario.Command.AlterarFuncionario;
using Acme.Sistemas.Services.V1.Funcionario.Command.CriarFuncionario;
using Acme.Sistemas.Services.V1.Funcionario.Command.ExcluirFuncionario;
using Acme.Sistemas.Services.V1.Funcionario.Query.ListarFuncionarios;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Funcionarios;

public sealed record AlterarFuncionarioRequest(
    string NomeCompleto, string? Email, string? Telefone,
    string? Cargo, string? Departamento, Guid? CentroDeCustoId,
    DateTime? DataAdmissao, DateTime? DataDemissao,
    Guid? UsuarioId, StatusAtivo Status);

public sealed class FuncionariosEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/funcionarios")
            .RequireAuthorization()
            .WithTags("Funcionarios");

        group.MapPost("/", async (CriarFuncionarioCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/funcionarios/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("CriarFuncionario").Produces<CriarFuncionarioCommandResult>(StatusCodes.Status201Created);

        group.MapPut("/{id:guid}", async (Guid id, AlterarFuncionarioRequest req, IMediator m, CancellationToken ct) =>
        {
            var cmd = new AlterarFuncionarioCommand(id, req.NomeCompleto, req.Email, req.Telefone,
                req.Cargo, req.Departamento, req.CentroDeCustoId,
                req.DataAdmissao, req.DataDemissao, req.UsuarioId, req.Status);
            var r = await m.Send(cmd, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("AlterarFuncionario").Produces<AlterarFuncionarioCommandResult>();

        group.MapDelete("/{id:guid}", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ExcluirFuncionarioCommand(id), ct);
            return r.IsSuccess ? Results.NoContent() : Results.Json(r, statusCode: r.Status);
        }).WithName("ExcluirFuncionario").Produces(StatusCodes.Status204NoContent);

        group.MapGet("/", async (int? skip, int? take, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ListarFuncionariosQuery(skip ?? 0, take ?? 100), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ListarFuncionarios").Produces<ListarFuncionariosQueryResult>();
    }
}
