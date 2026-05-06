using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Cliente.Command.AlterarCliente;
using Acme.Sistemas.Services.V1.Cliente.Command.AtualizarInadimplencia;
using Acme.Sistemas.Services.V1.Cliente.Command.CriarCliente;
using Acme.Sistemas.Services.V1.Cliente.Command.ExcluirCliente;
using Acme.Sistemas.Services.V1.Cliente.Query.ListarClientes;
using Acme.Sistemas.Services.V1.Cliente.Query.ObterCliente;
using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes;

public sealed record AlterarClienteRequest(
    TipoPessoa Tipo, string Nome, string? NomeFantasia, string Documento,
    string? InscricaoEstadual, string? Email, string? Telefone,
    StatusAtivo Status, EnderecoDto? Endereco);

public sealed record AtualizarInadimplenciaRequest(bool Inadimplente, bool BloquearVendas);

public sealed class ClientesEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/clientes")
            .RequireAuthorization()
            .WithTags("Clientes");

        group.MapPost("/", async (CriarClienteCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/clientes/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("CriarCliente").Produces<CriarClienteCommandResult>(StatusCodes.Status201Created);

        group.MapPut("/{id:guid}", async (Guid id, AlterarClienteRequest req, IMediator m, CancellationToken ct) =>
        {
            var cmd = new AlterarClienteCommand(id, req.Tipo, req.Nome, req.NomeFantasia,
                req.Documento, req.InscricaoEstadual, req.Email, req.Telefone, req.Status, req.Endereco);
            var r = await m.Send(cmd, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("AlterarCliente").Produces<AlterarClienteCommandResult>();

        group.MapDelete("/{id:guid}", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ExcluirClienteCommand(id), ct);
            return r.IsSuccess ? Results.NoContent() : Results.Json(r, statusCode: r.Status);
        }).WithName("ExcluirCliente").Produces(StatusCodes.Status204NoContent);

        group.MapPost("/{id:guid}/inadimplencia", async (Guid id, AtualizarInadimplenciaRequest req, IMediator m, CancellationToken ct) =>
        {
            var cmd = new AtualizarInadimplenciaCommand(id, req.Inadimplente, req.BloquearVendas);
            var r = await m.Send(cmd, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("AtualizarInadimplenciaCliente").Produces<AtualizarInadimplenciaCommandResult>();

        group.MapGet("/", async (string? termo, bool? inadimplente, int? skip, int? take, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ListarClientesQuery(termo, inadimplente, skip ?? 0, take ?? 50), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ListarClientes").Produces<ListarClientesQueryResult>();

        group.MapGet("/{id:guid}", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ObterClienteQuery(id), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ObterCliente").Produces<ObterClienteQueryResult>().ProducesProblem(404);
    }
}
