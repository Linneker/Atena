using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Empresa.Command.AlterarEmpresa;
using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Empresas.AlterarEmpresa;
public sealed record AlterarEmpresaRequest(
    string RazaoSocial,
    string? NomeFantasia,
    string Cnpj,
    string? InscricaoEstadual,
    string? InscricaoMunicipal,
    string? Email,
    string? Telefone,
    StatusAtivo Status,
    EnderecoDto? Endereco,
    bool BuscarEnderecoPorCep = false);

public sealed class AlterarEmpresaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/empresas/{id:guid}", async (
            Guid id,
            AlterarEmpresaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new AlterarEmpresaCommand(
                id, request.RazaoSocial, request.NomeFantasia, request.Cnpj,
                request.InscricaoEstadual, request.InscricaoMunicipal,
                request.Email, request.Telefone, request.Status,
                request.Endereco, request.BuscarEnderecoPorCep);

            var response = await mediator.Send(command, cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Empresas")
        .WithName("AlterarEmpresa")
        .Produces<AlterarEmpresaCommandResult>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
