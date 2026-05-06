using Acme.Sistemas.Core.Helper;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.ExternalIntegration.Clients.ViaCep;

namespace Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;

public sealed class CriarEmpresaCommandHandler
    : IRequestHandler<CriarEmpresaCommand, ResponseDefault<CriarEmpresaCommandResult>>
{
    private readonly IEmpresaRepository _empresas;
    private readonly IViaCepExternalClient _viaCep;
    private readonly ITenantContext _tenantContext;

    public CriarEmpresaCommandHandler(
        IEmpresaRepository empresas,
        IViaCepExternalClient viaCep,
        ITenantContext tenantContext)
    {
        _empresas = empresas;
        _viaCep = viaCep;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarEmpresaCommandResult>> Handle(
        CriarEmpresaCommand request,
        CancellationToken cancellationToken)
    {
        var cnpj = CnpjHelper.OnlyDigits(request.Cnpj);

        var existing = await _empresas.GetByCnpjAsync(cnpj, cancellationToken);
        if (existing is not null)
        {
            return ResponseDefault<CriarEmpresaCommandResult>.Conflict(
                $"Já existe uma empresa cadastrada com o CNPJ {cnpj}.");
        }

        var endereco = await BuildEnderecoAsync(request);

        var empresa = new Domain.Entities.Cadastros.Empresa
        {
            TenantId = _tenantContext.TenantId,
            RazaoSocial = request.RazaoSocial,
            NomeFantasia = request.NomeFantasia,
            Cnpj = cnpj,
            InscricaoEstadual = request.InscricaoEstadual,
            InscricaoMunicipal = request.InscricaoMunicipal,
            Email = request.Email,
            Telefone = request.Telefone,
            Endereco = endereco,
            CreatedBy = _tenantContext.UserId
        };

        await _empresas.AddAsync(empresa, cancellationToken);

        return ResponseDefault<CriarEmpresaCommandResult>.Created(
            new CriarEmpresaCommandResult(empresa.Id, empresa.RazaoSocial, empresa.Cnpj));
    }

    private async Task<Endereco> BuildEnderecoAsync(CriarEmpresaCommand request)
    {
        var endereco = new Endereco
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

        if (request.BuscarEnderecoPorCep && !string.IsNullOrWhiteSpace(endereco.Cep))
        {
            var cepDigits = new string(endereco.Cep.Where(char.IsDigit).ToArray());
            if (cepDigits.Length == 8)
            {
                var resp = await _viaCep.ConsultarPorCepAsync(cepDigits);
                if (resp.IsSuccess && resp.Content is not null && resp.Content.Erro != true)
                {
                    endereco.Logradouro ??= resp.Content.Logradouro;
                    endereco.Bairro ??= resp.Content.Bairro;
                    endereco.Cidade ??= resp.Content.Localidade;
                    endereco.Uf ??= resp.Content.Uf;
                }
            }
        }

        return endereco;
    }
}
