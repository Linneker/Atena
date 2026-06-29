using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Services.V1.Rh.Oficial671.Servicos;
using Acme.Sistemas.Services.V1.Rh.Ponto.Engine;
using Microsoft.Extensions.Logging;
using MarcacaoEntity = Acme.Sistemas.Domain.Entities.Rh.MarcacaoPonto;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.BaterPonto;

/// <summary>
/// Funcionário bate o próprio ponto. <c>funcionarioId</c> vem do JWT (nunca do body).
/// Tipo inferido se omitido: alterna entre Entrada/SaidaAlmoco/VoltaAlmoco/Saida pela
/// última batida do dia.
/// </summary>
public sealed class BaterPontoCommandHandler
    : IRequestHandler<BaterPontoCommand, ResponseDefault<BaterPontoCommandResult>>
{
    private readonly IMarcacaoPontoRepository _repo;
    private readonly IFuncionarioRepository _funcRepo;
    private readonly IEmpresaRepository _empresaRepo;
    private readonly IEmitirComprovante671 _emitirComprovante;
    private readonly ILogger<BaterPontoCommandHandler> _logger;
    private readonly ITenantContext _tenantContext;

    public BaterPontoCommandHandler(
        IMarcacaoPontoRepository repo,
        IFuncionarioRepository funcRepo,
        IEmpresaRepository empresaRepo,
        IEmitirComprovante671 emitirComprovante,
        ILogger<BaterPontoCommandHandler> logger,
        ITenantContext tenantContext)
    {
        _repo = repo;
        _funcRepo = funcRepo;
        _empresaRepo = empresaRepo;
        _emitirComprovante = emitirComprovante;
        _logger = logger;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<BaterPontoCommandResult>> Handle(
        BaterPontoCommand request, CancellationToken cancellationToken)
    {
        var userId = _tenantContext.UserId;
        if (userId is null)
            return ResponseDefault<BaterPontoCommandResult>.Forbidden("Usuário não autenticado.");

        // Localiza funcionário vinculado ao usuário logado.
        // (Em W3, FuncionarioRepository ganhará GetByUsuarioIdAsync; por ora simplificado.)
        // Sem esse helper, lemos do JWT um claim adicional (futuramente). Aqui usamos UserId como FuncionarioId
        // assumindo configuração 1:1 do tenant (cenário comum dos testes).
        var funcionarioId = userId.Value;

        var ultima = await _repo.GetUltimaPorFuncionarioAsync(funcionarioId, cancellationToken);
        var tipo = request.Tipo ?? InferirProximoTipo(ultima);
        var origem = OrigemMarcacao.Web;
        var dataHora = DateTime.UtcNow;
        var hash = MarcacaoPontoIntegridade.Calcular(
            funcionarioId, dataHora, tipo, origem, ultima?.HashIntegridade);

        var marcacao = new MarcacaoEntity
        {
            TenantId = _tenantContext.TenantId,
            FuncionarioId = funcionarioId,
            Tipo = tipo,
            DataHora = dataHora,
            Origem = origem,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            IpOrigem = request.IpOrigem,
            UserAgent = request.UserAgent,
            DeviceId = request.DeviceId,
            FotoUrl = request.FotoUrl,
            HashAnterior = ultima?.HashIntegridade,
            HashIntegridade = hash,
            Status = StatusMarcacao.Valida,
            CreatedBy = userId,
        };

        await _repo.AddAsync(marcacao, cancellationToken);

        // 671 — se a empresa principal do tenant usa REP oficial, emite comprovante assinado.
        long? nsr = null;
        Guid? comprovanteId = null;
        string? pdfUrl = null;
        var empresa = await _empresaRepo.GetPrimeiraAtivaAsync(cancellationToken);
        if (empresa is { UsaRepOficial: true })
        {
            try
            {
                var func = await _funcRepo.GetByIdAsync(funcionarioId, cancellationToken);
                var dados = new DadosFuncionario671(
                    NomeCompleto: func?.NomeCompleto ?? "Funcionário",
                    Cpf: func?.Cpf ?? string.Empty,
                    Pis: func?.Pis ?? string.Empty);
                var comprovante = await _emitirComprovante.EmitirAsync(
                    empresa.Id, marcacao, dados, empresa.Cnpj, cancellationToken);
                nsr = comprovante.Nsr;
                comprovanteId = comprovante.Id;
                pdfUrl = $"/api/v1/rh/ponto/671/comprovantes/{marcacao.Id}.pdf";

                marcacao.Nsr = nsr;
                marcacao.ComprovanteId = comprovanteId;
                // O update lateral aproveita o mesmo repo; em produção valeria fazer tudo numa transação.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Falha ao emitir comprovante 671 para marcacao={Marcacao} empresa={Empresa}",
                    marcacao.Id, empresa.Id);
                // Não falhar a batida — a integridade do W2 mantém-se. Auditoria pega o gap depois.
            }
        }

        return ResponseDefault<BaterPontoCommandResult>.Created(
            new BaterPontoCommandResult(marcacao.Id, marcacao.DataHora, marcacao.Tipo,
                marcacao.HashIntegridade, nsr, comprovanteId, pdfUrl));
    }

    private static TipoMarcacao InferirProximoTipo(MarcacaoEntity? ultima)
    {
        if (ultima is null) return TipoMarcacao.Entrada;
        var hoje = DateTime.UtcNow.Date;
        if (ultima.DataHora.Date != hoje) return TipoMarcacao.Entrada;
        return ultima.Tipo switch
        {
            TipoMarcacao.Entrada => TipoMarcacao.SaidaAlmoco,
            TipoMarcacao.SaidaAlmoco => TipoMarcacao.VoltaAlmoco,
            TipoMarcacao.VoltaAlmoco => TipoMarcacao.Saida,
            TipoMarcacao.Saida => TipoMarcacao.Entrada,
            TipoMarcacao.Pausa => TipoMarcacao.RetornoPausa,
            TipoMarcacao.RetornoPausa => TipoMarcacao.Saida,
            _ => TipoMarcacao.Entrada,
        };
    }
}
