using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Rh.Oficial671;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Command.SalvarConfiguracaoRep;

/// <summary>
/// Upsert da <c>ConfiguracaoRep</c> da empresa do tenant. Idempotente por (tenant, empresa).
/// Sem essa configuração completa, a empresa não pode ativar <c>usa_rep_oficial</c>.
/// </summary>
public sealed class SalvarConfiguracaoRepCommandHandler
    : IRequestHandler<SalvarConfiguracaoRepCommand, ResponseDefault<SalvarConfiguracaoRepCommandResult>>
{
    private readonly IConfiguracaoRepRepository _repo;
    private readonly ITenantContext _tenant;

    public SalvarConfiguracaoRepCommandHandler(IConfiguracaoRepRepository repo, ITenantContext tenant)
    {
        _repo = repo;
        _tenant = tenant;
    }

    public async Task<ResponseDefault<SalvarConfiguracaoRepCommandResult>> Handle(
        SalvarConfiguracaoRepCommand r, CancellationToken cancellationToken)
    {
        var existing = await _repo.GetByEmpresaAsync(r.EmpresaId, cancellationToken);
        var criada = existing is null;
        var c = existing ?? new ConfiguracaoRep
        {
            TenantId = _tenant.TenantId,
            EmpresaId = r.EmpresaId,
            CreatedBy = _tenant.UserId,
        };
        c.Tipo = r.Tipo;
        c.RazaoSocial = r.RazaoSocial;
        c.CnpjCei = r.CnpjCei;
        c.Cno = r.Cno;
        c.InscricaoEstadual = r.InscricaoEstadual;
        c.CnaePrincipal = r.CnaePrincipal;
        c.EnderecoLogradouro = r.EnderecoLogradouro;
        c.EnderecoNumero = r.EnderecoNumero;
        c.EnderecoComplemento = r.EnderecoComplemento;
        c.EnderecoBairro = r.EnderecoBairro;
        c.EnderecoCidade = r.EnderecoCidade;
        c.EnderecoUf = r.EnderecoUf;
        c.EnderecoCep = r.EnderecoCep;
        c.CertificadoId = r.CertificadoId;
        c.ResponsavelCpf = r.ResponsavelCpf;
        c.ResponsavelNome = r.ResponsavelNome;
        c.UpdatedBy = _tenant.UserId;

        if (criada) await _repo.AddAsync(c, cancellationToken);
        else await _repo.UpdateAsync(c, cancellationToken);

        return ResponseDefault<SalvarConfiguracaoRepCommandResult>.Ok(
            new SalvarConfiguracaoRepCommandResult(c.Id, criada));
    }
}
