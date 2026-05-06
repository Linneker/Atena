using Acme.Sistemas.Core.Helper;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.ExternalIntegration.Clients.ViaCep;
using ClienteEntity = Acme.Sistemas.Domain.Entities.Cadastros.Cliente;

namespace Acme.Sistemas.Services.V1.Cliente.Command.CriarCliente;

public sealed class CriarClienteCommandHandler
    : IRequestHandler<CriarClienteCommand, ResponseDefault<CriarClienteCommandResult>>
{
    private readonly IClienteRepository _repo;
    private readonly IViaCepExternalClient _viaCep;
    private readonly ITenantContext _tenantContext;

    public CriarClienteCommandHandler(
        IClienteRepository repo, IViaCepExternalClient viaCep, ITenantContext tenantContext)
    {
        _repo = repo;
        _viaCep = viaCep;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarClienteCommandResult>> Handle(CriarClienteCommand request, CancellationToken cancellationToken)
    {
        var documento = DocumentoHelper.OnlyDigits(request.Documento);
        var existing = await _repo.GetByDocumentoAsync(documento, cancellationToken);
        if (existing is not null)
            return ResponseDefault<CriarClienteCommandResult>.Conflict(
                $"Já existe um cliente com o documento {documento}.");

        var endereco = await BuildEnderecoAsync(request, cancellationToken);

        var cliente = new ClienteEntity
        {
            TenantId = _tenantContext.TenantId,
            Tipo = request.Tipo,
            Nome = request.Nome,
            NomeFantasia = request.NomeFantasia,
            Documento = documento,
            InscricaoEstadual = request.InscricaoEstadual,
            Email = request.Email,
            Telefone = request.Telefone,
            Status = StatusAtivo.Ativo,
            Endereco = endereco,
            CreatedBy = _tenantContext.UserId
        };

        await _repo.AddAsync(cliente, cancellationToken);

        return ResponseDefault<CriarClienteCommandResult>.Created(
            new CriarClienteCommandResult(cliente.Id, cliente.Nome, cliente.Documento));
    }

    private async Task<Endereco> BuildEnderecoAsync(CriarClienteCommand request, CancellationToken ct)
    {
        var e = new Endereco
        {
            Cep = request.Endereco?.Cep,
            Logradouro = request.Endereco?.Logradouro,
            Numero = request.Endereco?.Numero,
            Complemento = request.Endereco?.Complemento,
            Bairro = request.Endereco?.Bairro,
            Cidade = request.Endereco?.Cidade,
            Uf = request.Endereco?.Uf,
            Pais = "BR"
        };

        if (request.BuscarEnderecoPorCep && !string.IsNullOrWhiteSpace(e.Cep))
        {
            var cepDigits = new string(e.Cep.Where(char.IsDigit).ToArray());
            if (cepDigits.Length == 8)
            {
                var resp = await _viaCep.ConsultarPorCepAsync(cepDigits);
                if (resp.IsSuccess && resp.Content is not null && resp.Content.Erro != true)
                {
                    e.Logradouro ??= resp.Content.Logradouro;
                    e.Bairro ??= resp.Content.Bairro;
                    e.Cidade ??= resp.Content.Localidade;
                    e.Uf ??= resp.Content.Uf;
                }
            }
        }

        return e;
    }
}
