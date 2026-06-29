using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Rh.Oficial671;
using Acme.Sistemas.Domain.Enums.Rh;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Services.V1.Rh.Oficial671.Afd;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Command.ExportarAfd;

/// <summary>
/// Geração síncrona MVP do AFD: monta o arquivo, calcula hash, persiste metadados.
/// Upload em S3 fica como TODO — por enquanto guardamos um content URL data:application/octet-stream
/// gerado on-demand pelo endpoint download. Worker assíncrono via RabbitMQ é follow-up (`AfdExportWorker`).
/// </summary>
public sealed class ExportarAfdCommandHandler
    : IRequestHandler<ExportarAfdCommand, ResponseDefault<ExportarAfdCommandResult>>
{
    private readonly IExportacaoAfdRepository _expRepo;
    private readonly IEmpresaRepository _empresas;
    private readonly IConfiguracaoRepRepository _config;
    private readonly IMarcacaoPontoRepository _marcacoes;
    private readonly IComprovantePontoRepository _comprovantes;
    private readonly IFuncionarioRepository _funcionarios;
    private readonly LayoutAfd003Writer _writer;
    private readonly ITenantContext _tenant;

    public ExportarAfdCommandHandler(
        IExportacaoAfdRepository expRepo,
        IEmpresaRepository empresas,
        IConfiguracaoRepRepository config,
        IMarcacaoPontoRepository marcacoes,
        IComprovantePontoRepository comprovantes,
        IFuncionarioRepository funcionarios,
        LayoutAfd003Writer writer,
        ITenantContext tenant)
    {
        _expRepo = expRepo;
        _empresas = empresas;
        _config = config;
        _marcacoes = marcacoes;
        _comprovantes = comprovantes;
        _funcionarios = funcionarios;
        _writer = writer;
        _tenant = tenant;
    }

    public async Task<ResponseDefault<ExportarAfdCommandResult>> Handle(
        ExportarAfdCommand r, CancellationToken cancellationToken)
    {
        var empresa = await _empresas.GetByIdAsync(r.EmpresaId, cancellationToken);
        if (empresa is null)
            return ResponseDefault<ExportarAfdCommandResult>.NotFound("Empresa não encontrada.");

        var cfg = await _config.GetByEmpresaAsync(r.EmpresaId, cancellationToken);
        if (cfg is null)
            return ResponseDefault<ExportarAfdCommandResult>.Conflict(
                "ConfiguracaoRep ausente — cadastre antes de exportar AFD.");

        var exportacao = new ExportacaoAfd
        {
            TenantId = _tenant.TenantId,
            EmpresaId = r.EmpresaId,
            PeriodoInicio = r.PeriodoInicio,
            PeriodoFim = r.PeriodoFim,
            LayoutVersao = "003",
            Status = StatusExportacao671.Processando,
            CreatedBy = _tenant.UserId,
        };
        await _expRepo.AddAsync(exportacao, cancellationToken);

        try
        {
            var inicio = r.PeriodoInicio.ToDateTime(TimeOnly.MinValue);
            var fim = r.PeriodoFim.AddDays(1).ToDateTime(TimeOnly.MinValue);

            var comprovantes = await _comprovantes.ListByEmpresaPeriodoAsync(
                r.EmpresaId, inicio, fim, cancellationToken);

            var marcacaoIds = comprovantes.Select(c => c.MarcacaoId).ToHashSet();
            var marcacoes = new List<Domain.Entities.Rh.MarcacaoPonto>();
            foreach (var mid in marcacaoIds)
            {
                var m = await _marcacoes.GetByIdAsync(mid, cancellationToken);
                if (m is not null) marcacoes.Add(m);
            }

            var funcionariosIds = marcacoes.Select(m => m.FuncionarioId).Distinct().ToList();
            var funcionarios = new List<Domain.Entities.Cadastros.Funcionario>();
            foreach (var fid in funcionariosIds)
            {
                var f = await _funcionarios.GetByIdAsync(fid, cancellationToken);
                if (f is not null) funcionarios.Add(f);
            }

            var ctx = new AfdContexto(
                empresa, cfg,
                PeriodoInicio: inicio, PeriodoFim: fim,
                GeradoEm: DateTime.UtcNow,
                Marcacoes: marcacoes, Comprovantes: comprovantes, Funcionarios: funcionarios);
            var afd = _writer.Escrever(ctx);

            // Upload S3 stub: guardamos um URL "memorial" e os bytes ficam disponíveis via
            // refetch dos comprovantes do período (idempotência). Substituir por S3 real em PR
            // `rh-671-afd-s3-storage`.
            var url = $"s3://atena-rh-afd/{_tenant.TenantId}/{r.EmpresaId}/" +
                      $"{r.PeriodoInicio:yyyyMMdd}-{r.PeriodoFim:yyyyMMdd}.txt";

            exportacao.Status = StatusExportacao671.Concluida;
            exportacao.ArquivoUrl = url;
            exportacao.HashSha256 = afd.HashSha256Hex;
            exportacao.GeradoEm = DateTime.UtcNow;
            exportacao.UpdatedBy = _tenant.UserId;
            await _expRepo.UpdateAsync(exportacao, cancellationToken);

            return ResponseDefault<ExportarAfdCommandResult>.Ok(new ExportarAfdCommandResult(
                exportacao.Id, exportacao.Status.ToString(), exportacao.ArquivoUrl, exportacao.HashSha256));
        }
        catch (Exception ex)
        {
            exportacao.Status = StatusExportacao671.Falhou;
            exportacao.Erro = ex.Message;
            exportacao.UpdatedBy = _tenant.UserId;
            await _expRepo.UpdateAsync(exportacao, cancellationToken);
            throw;
        }
    }
}
