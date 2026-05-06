using Acme.Sistemas.Core.Helper;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.ExternalIntegration.Clients.ViaCep;

namespace Acme.Sistemas.Services.V1.Empresa.Command.AlterarEmpresa;

public sealed class AlterarEmpresaCommandHandler
    : IRequestHandler<AlterarEmpresaCommand, ResponseDefault<AlterarEmpresaCommandResult>>
{
    private readonly IEmpresaRepository _empresas;
    private readonly IViaCepExternalClient _viaCep;
    private readonly ITenantContext _tenantContext;

    public AlterarEmpresaCommandHandler(
        IEmpresaRepository empresas,
        IViaCepExternalClient viaCep,
        ITenantContext tenantContext)
    {
        _empresas = empresas;
        _viaCep = viaCep;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarEmpresaCommandResult>> Handle(
        AlterarEmpresaCommand request,
        CancellationToken cancellationToken)
    {
        var empresa = await _empresas.GetByIdAsync(request.Id, cancellationToken);
        if (empresa is null)
        {
            return ResponseDefault<AlterarEmpresaCommandResult>.NotFound("Empresa não encontrada.");
        }

        var cnpj = CnpjHelper.OnlyDigits(request.Cnpj);
        if (!string.Equals(empresa.Cnpj, cnpj, StringComparison.Ordinal))
        {
            var existing = await _empresas.GetByCnpjAsync(cnpj, cancellationToken);
            if (existing is not null && existing.Id != empresa.Id)
            {
                return ResponseDefault<AlterarEmpresaCommandResult>.Conflict(
                    $"Já existe uma empresa cadastrada com o CNPJ {cnpj}.");
            }
        }

        empresa.RazaoSocial = request.RazaoSocial;
        empresa.NomeFantasia = request.NomeFantasia;
        empresa.Cnpj = cnpj;
        empresa.InscricaoEstadual = request.InscricaoEstadual;
        empresa.InscricaoMunicipal = request.InscricaoMunicipal;
        empresa.Email = request.Email;
        empresa.Telefone = request.Telefone;
        empresa.Status = request.Status;
        empresa.Endereco = await BuildEnderecoAsync(request, empresa.Endereco);
        empresa.UpdatedBy = _tenantContext.UserId;

        await _empresas.UpdateAsync(empresa, cancellationToken);

        return ResponseDefault<AlterarEmpresaCommandResult>.Ok(new AlterarEmpresaCommandResult(empresa.Id));
    }

    private async Task<Endereco> BuildEnderecoAsync(AlterarEmpresaCommand request, Endereco fallback)
    {
        var endereco = new Endereco
        {
            Cep = request.Endereco?.Cep ?? fallback.Cep,
            Logradouro = request.Endereco?.Logradouro ?? fallback.Logradouro,
            Numero = request.Endereco?.Numero ?? fallback.Numero,
            Complemento = request.Endereco?.Complemento ?? fallback.Complemento,
            Bairro = request.Endereco?.Bairro ?? fallback.Bairro,
            Cidade = request.Endereco?.Cidade ?? fallback.Cidade,
            Uf = request.Endereco?.Uf ?? fallback.Uf,
            Pais = fallback.Pais ?? "BR"
        };

        if (request.BuscarEnderecoPorCep && !string.IsNullOrWhiteSpace(endereco.Cep))
        {
            var cepDigits = new string(endereco.Cep.Where(char.IsDigit).ToArray());
            if (cepDigits.Length == 8)
            {
                var resp = await _viaCep.ConsultarPorCepAsync(cepDigits);
                if (resp.IsSuccess && resp.Content is not null && resp.Content.Erro != true)
                {
                    endereco.Logradouro = resp.Content.Logradouro ?? endereco.Logradouro;
                    endereco.Bairro = resp.Content.Bairro ?? endereco.Bairro;
                    endereco.Cidade = resp.Content.Localidade ?? endereco.Cidade;
                    endereco.Uf = resp.Content.Uf ?? endereco.Uf;
                }
            }
        }

        return endereco;
    }
}
