using Acme.Sistemas.Core.Helper;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using FornecedorEntity = Acme.Sistemas.Domain.Entities.Cadastros.Fornecedor;

namespace Acme.Sistemas.Services.V1.Fornecedor.Command.CriarFornecedor;

public sealed class CriarFornecedorCommandHandler
    : IRequestHandler<CriarFornecedorCommand, ResponseDefault<CriarFornecedorCommandResult>>
{
    private readonly IFornecedorRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarFornecedorCommandHandler(IFornecedorRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarFornecedorCommandResult>> Handle(CriarFornecedorCommand request, CancellationToken cancellationToken)
    {
        var documento = DocumentoHelper.OnlyDigits(request.Documento);
        var existing = await _repo.GetByDocumentoAsync(documento, cancellationToken);
        if (existing is not null)
            return ResponseDefault<CriarFornecedorCommandResult>.Conflict(
                $"Já existe um fornecedor com o documento {documento}.");

        var fornecedor = new FornecedorEntity
        {
            TenantId = _tenantContext.TenantId,
            Tipo = request.Tipo,
            Nome = request.Nome,
            NomeFantasia = request.NomeFantasia,
            Documento = documento,
            InscricaoEstadual = request.InscricaoEstadual,
            Email = request.Email,
            Telefone = request.Telefone,
            CondicaoPagamentoPadrao = request.CondicaoPagamentoPadrao,
            Status = StatusAtivo.Ativo,
            Endereco = new Endereco
            {
                Cep = request.Endereco?.Cep,
                Logradouro = request.Endereco?.Logradouro,
                Numero = request.Endereco?.Numero,
                Complemento = request.Endereco?.Complemento,
                Bairro = request.Endereco?.Bairro,
                Cidade = request.Endereco?.Cidade,
                Uf = request.Endereco?.Uf,
                Pais = "BR"
            },
            CreatedBy = _tenantContext.UserId
        };

        await _repo.AddAsync(fornecedor, cancellationToken);
        return ResponseDefault<CriarFornecedorCommandResult>.Created(
            new CriarFornecedorCommandResult(fornecedor.Id, fornecedor.Nome, fornecedor.Documento));
    }
}
