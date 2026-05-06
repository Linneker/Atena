using Acme.Sistemas.Core.Helper;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Fornecedor.Command.AlterarFornecedor;

public sealed class AlterarFornecedorCommandHandler
    : IRequestHandler<AlterarFornecedorCommand, ResponseDefault<AlterarFornecedorCommandResult>>
{
    private readonly IFornecedorRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AlterarFornecedorCommandHandler(IFornecedorRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarFornecedorCommandResult>> Handle(AlterarFornecedorCommand request, CancellationToken cancellationToken)
    {
        var fornecedor = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (fornecedor is null)
            return ResponseDefault<AlterarFornecedorCommandResult>.NotFound("Fornecedor não encontrado.");

        var documento = DocumentoHelper.OnlyDigits(request.Documento);
        if (!string.Equals(fornecedor.Documento, documento, StringComparison.Ordinal))
        {
            var existing = await _repo.GetByDocumentoAsync(documento, cancellationToken);
            if (existing is not null && existing.Id != fornecedor.Id)
                return ResponseDefault<AlterarFornecedorCommandResult>.Conflict(
                    $"Já existe outro fornecedor com o documento {documento}.");
        }

        fornecedor.Tipo = request.Tipo;
        fornecedor.Nome = request.Nome;
        fornecedor.NomeFantasia = request.NomeFantasia;
        fornecedor.Documento = documento;
        fornecedor.InscricaoEstadual = request.InscricaoEstadual;
        fornecedor.Email = request.Email;
        fornecedor.Telefone = request.Telefone;
        fornecedor.CondicaoPagamentoPadrao = request.CondicaoPagamentoPadrao;
        fornecedor.Status = request.Status;
        fornecedor.Endereco = new Endereco
        {
            Cep = request.Endereco?.Cep ?? fornecedor.Endereco.Cep,
            Logradouro = request.Endereco?.Logradouro ?? fornecedor.Endereco.Logradouro,
            Numero = request.Endereco?.Numero ?? fornecedor.Endereco.Numero,
            Complemento = request.Endereco?.Complemento ?? fornecedor.Endereco.Complemento,
            Bairro = request.Endereco?.Bairro ?? fornecedor.Endereco.Bairro,
            Cidade = request.Endereco?.Cidade ?? fornecedor.Endereco.Cidade,
            Uf = request.Endereco?.Uf ?? fornecedor.Endereco.Uf,
            Pais = fornecedor.Endereco.Pais ?? "BR"
        };
        fornecedor.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(fornecedor, cancellationToken);
        return ResponseDefault<AlterarFornecedorCommandResult>.Ok(new AlterarFornecedorCommandResult(fornecedor.Id));
    }
}
