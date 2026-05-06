using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;
using Acme.Sistemas.Services.V1.Fornecedor.Command.AlterarFornecedor;
using Acme.Sistemas.Services.V1.Fornecedor.Command.CriarFornecedor;
using Acme.Sistemas.Services.V1.Fornecedor.Command.ExcluirFornecedor;
using Acme.Sistemas.Services.V1.Fornecedor.Query.ListarFornecedores;
using Acme.Sistemas.Services.V1.Fornecedor.Query.ObterFornecedor;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fornecedores;

public sealed record AlterarFornecedorRequest(
    TipoPessoa Tipo, string Nome, string? NomeFantasia, string Documento,
    string? InscricaoEstadual, string? Email, string? Telefone,
    string? CondicaoPagamentoPadrao, StatusAtivo Status, EnderecoDto? Endereco);

public sealed class FornecedoresEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/fornecedores")
            .RequireAuthorization()
            .WithTags("Fornecedores");

        group.MapPost("/", async (CriarFornecedorCommand cmd, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(cmd, ct);
            return r.IsSuccess
                ? Results.Created($"/api/v1/fornecedores/{r.Content!.Id}", r.Content)
                : Results.Json(r, statusCode: r.Status);
        }).WithName("CriarFornecedor").Produces<CriarFornecedorCommandResult>(StatusCodes.Status201Created);

        group.MapPut("/{id:guid}", async (Guid id, AlterarFornecedorRequest req, IMediator m, CancellationToken ct) =>
        {
            var cmd = new AlterarFornecedorCommand(id, req.Tipo, req.Nome, req.NomeFantasia,
                req.Documento, req.InscricaoEstadual, req.Email, req.Telefone,
                req.CondicaoPagamentoPadrao, req.Status, req.Endereco);
            var r = await m.Send(cmd, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("AlterarFornecedor").Produces<AlterarFornecedorCommandResult>();

        group.MapDelete("/{id:guid}", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ExcluirFornecedorCommand(id), ct);
            return r.IsSuccess ? Results.NoContent() : Results.Json(r, statusCode: r.Status);
        }).WithName("ExcluirFornecedor").Produces(StatusCodes.Status204NoContent);

        group.MapGet("/", async (string? termo, int? skip, int? take, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ListarFornecedoresQuery(termo, skip ?? 0, take ?? 50), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ListarFornecedores").Produces<ListarFornecedoresQueryResult>();

        group.MapGet("/{id:guid}", async (Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new ObterFornecedorQuery(id), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ObterFornecedor").Produces<ObterFornecedorQueryResult>().ProducesProblem(404);
    }
}
