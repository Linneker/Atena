using System.Security.Cryptography;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Rh.Oficial671;
using Acme.Sistemas.Domain.Enums.Rh;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.ExternalIntegration.Sefaz.Certificado;
using Acme.Sistemas.Services.V1.Rh.Oficial671.Aej;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Command.ExportarAej;

public sealed class ExportarAejCommandHandler
    : IRequestHandler<ExportarAejCommand, ResponseDefault<ExportarAejCommandResult>>
{
    private readonly IExportacaoAejRepository _expRepo;
    private readonly IEmpresaRepository _empresas;
    private readonly IConfiguracaoRepRepository _config;
    private readonly IMarcacaoPontoRepository _marcacoes;
    private readonly IComprovantePontoRepository _comprovantes;
    private readonly IFuncionarioRepository _funcionarios;
    private readonly GeradorAejV1 _gerador;
    private readonly AssinadorAej _assinador;
    private readonly CertificadoTenantResolver _certResolver;
    private readonly ITenantContext _tenant;

    public ExportarAejCommandHandler(
        IExportacaoAejRepository expRepo, IEmpresaRepository empresas,
        IConfiguracaoRepRepository config, IMarcacaoPontoRepository marcacoes,
        IComprovantePontoRepository comprovantes, IFuncionarioRepository funcionarios,
        GeradorAejV1 gerador, AssinadorAej assinador,
        CertificadoTenantResolver certResolver, ITenantContext tenant)
    {
        _expRepo = expRepo; _empresas = empresas; _config = config;
        _marcacoes = marcacoes; _comprovantes = comprovantes; _funcionarios = funcionarios;
        _gerador = gerador; _assinador = assinador;
        _certResolver = certResolver; _tenant = tenant;
    }

    public async Task<ResponseDefault<ExportarAejCommandResult>> Handle(
        ExportarAejCommand r, CancellationToken cancellationToken)
    {
        var empresa = await _empresas.GetByIdAsync(r.EmpresaId, cancellationToken);
        if (empresa is null)
            return ResponseDefault<ExportarAejCommandResult>.NotFound("Empresa não encontrada.");
        var cfg = await _config.GetByEmpresaAsync(r.EmpresaId, cancellationToken);
        if (cfg is null)
            return ResponseDefault<ExportarAejCommandResult>.Conflict("ConfiguracaoRep ausente.");

        var exp = new ExportacaoAej
        {
            TenantId = _tenant.TenantId,
            EmpresaId = r.EmpresaId,
            PeriodoInicio = r.PeriodoInicio,
            PeriodoFim = r.PeriodoFim,
            LayoutVersao = "v1",
            Status = StatusExportacao671.Processando,
            CreatedBy = _tenant.UserId,
        };
        await _expRepo.AddAsync(exp, cancellationToken);

        try
        {
            var inicio = r.PeriodoInicio.ToDateTime(TimeOnly.MinValue);
            var fim = r.PeriodoFim.AddDays(1).ToDateTime(TimeOnly.MinValue);
            var compList = await _comprovantes.ListByEmpresaPeriodoAsync(r.EmpresaId, inicio, fim, cancellationToken);
            var marcacaoIds = compList.Select(c => c.MarcacaoId).ToHashSet();
            var marcList = new List<Acme.Sistemas.Domain.Entities.Rh.MarcacaoPonto>();
            foreach (var mid in marcacaoIds)
            {
                var m = await _marcacoes.GetByIdAsync(mid, cancellationToken);
                if (m is not null) marcList.Add(m);
            }
            var funcIds = marcList.Select(m => m.FuncionarioId).Distinct().ToList();
            var funcList = new List<Acme.Sistemas.Domain.Entities.Cadastros.Funcionario>();
            foreach (var fid in funcIds)
            {
                var f = await _funcionarios.GetByIdAsync(fid, cancellationToken);
                if (f is not null) funcList.Add(f);
            }

            var ctx = new AejContexto(empresa, cfg, inicio, fim, DateTime.UtcNow,
                marcList, compList, funcList);
            var bytes = _gerador.Gerar(ctx);
            var hash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();

            string? signature = null;
            try
            {
                var cert = await _certResolver.GetAsync(cancellationToken);
                signature = _assinador.AssinarDetached(bytes, cert);
            }
            catch
            {
                // Sem cert disponível ainda — exportação fica não assinada (status Concluida com warn).
                exp.Erro = "AEJ gerado sem assinatura: cert ICP-Brasil indisponível.";
            }

            var urlBase = $"s3://atena-rh-aej/{_tenant.TenantId}/{r.EmpresaId}/{r.PeriodoInicio:yyyyMMdd}-{r.PeriodoFim:yyyyMMdd}";
            exp.Status = StatusExportacao671.Concluida;
            exp.ArquivoUrl = urlBase + ".json";
            exp.AssinaturaUrl = signature is null ? null : urlBase + ".jws";
            exp.HashSha256 = hash;
            exp.GeradoEm = DateTime.UtcNow;
            exp.UpdatedBy = _tenant.UserId;
            await _expRepo.UpdateAsync(exp, cancellationToken);

            return ResponseDefault<ExportarAejCommandResult>.Ok(new ExportarAejCommandResult(
                exp.Id, exp.Status.ToString(), exp.ArquivoUrl, exp.AssinaturaUrl, exp.HashSha256));
        }
        catch (Exception ex)
        {
            exp.Status = StatusExportacao671.Falhou;
            exp.Erro = ex.Message;
            exp.UpdatedBy = _tenant.UserId;
            await _expRepo.UpdateAsync(exp, cancellationToken);
            throw;
        }
    }
}
