using Acme.Sistemas.Core.Helper;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Cliente.Command.AlterarCliente;

public sealed class AlterarClienteCommandHandler
    : IRequestHandler<AlterarClienteCommand, ResponseDefault<AlterarClienteCommandResult>>
{
    private readonly IClienteRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AlterarClienteCommandHandler(IClienteRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarClienteCommandResult>> Handle(AlterarClienteCommand request, CancellationToken cancellationToken)
    {
        var cliente = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (cliente is null)
            return ResponseDefault<AlterarClienteCommandResult>.NotFound("Cliente não encontrado.");

        var documento = DocumentoHelper.OnlyDigits(request.Documento);
        if (!string.Equals(cliente.Documento, documento, StringComparison.Ordinal))
        {
            var existing = await _repo.GetByDocumentoAsync(documento, cancellationToken);
            if (existing is not null && existing.Id != cliente.Id)
                return ResponseDefault<AlterarClienteCommandResult>.Conflict(
                    $"Já existe outro cliente com o documento {documento}.");
        }

        cliente.Tipo = request.Tipo;
        cliente.Nome = request.Nome;
        cliente.NomeFantasia = request.NomeFantasia;
        cliente.Documento = documento;
        cliente.InscricaoEstadual = request.InscricaoEstadual;
        cliente.Email = request.Email;
        cliente.Telefone = request.Telefone;
        cliente.Status = request.Status;
        cliente.Endereco = new Endereco
        {
            Cep = request.Endereco?.Cep ?? cliente.Endereco.Cep,
            Logradouro = request.Endereco?.Logradouro ?? cliente.Endereco.Logradouro,
            Numero = request.Endereco?.Numero ?? cliente.Endereco.Numero,
            Complemento = request.Endereco?.Complemento ?? cliente.Endereco.Complemento,
            Bairro = request.Endereco?.Bairro ?? cliente.Endereco.Bairro,
            Cidade = request.Endereco?.Cidade ?? cliente.Endereco.Cidade,
            Uf = request.Endereco?.Uf ?? cliente.Endereco.Uf,
            Pais = cliente.Endereco.Pais ?? "BR"
        };
        cliente.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(cliente, cancellationToken);
        return ResponseDefault<AlterarClienteCommandResult>.Ok(new AlterarClienteCommandResult(cliente.Id));
    }
}
