using System.Security.Cryptography;
using System.Text;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Services.V1.Rh.Ponto.Engine;
using MarcacaoEntity = Acme.Sistemas.Domain.Entities.Rh.MarcacaoPonto;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.BaterPontoMobile;

public sealed class BaterPontoMobileCommandHandler
    : IRequestHandler<BaterPontoMobileCommand, ResponseDefault<BaterPontoMobileCommandResult>>
{
    private static readonly TimeSpan ToleranciaRelogio = TimeSpan.FromMinutes(5);

    private readonly IMarcacaoPontoRepository _repo;
    private readonly IDispositivoMobileRepository _dispositivos;
    private readonly ITenantContext _tenantContext;

    public BaterPontoMobileCommandHandler(
        IMarcacaoPontoRepository repo,
        IDispositivoMobileRepository dispositivos,
        ITenantContext tenantContext)
    {
        _repo = repo;
        _dispositivos = dispositivos;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<BaterPontoMobileCommandResult>> Handle(
        BaterPontoMobileCommand request, CancellationToken cancellationToken)
    {
        var userId = _tenantContext.UserId;
        if (userId is null)
            return ResponseDefault<BaterPontoMobileCommandResult>.Forbidden("Usuário não autenticado.");

        // 1. Foto OU prova biométrica obrigatórias
        if ((request.FotoBytes is null || request.FotoBytes.Length == 0)
            && string.IsNullOrWhiteSpace(request.ProvaBiometriaLocal))
            return ResponseDefault<BaterPontoMobileCommandResult>.BadRequest(
                Core.Response.Erros.Error.Validation("foto OU provaBiometriaLocal obrigatórios."));

        // 2. Dispositivo registrado + ativo
        var dispositivo = await _dispositivos.GetByDeviceIdAsync(userId.Value, request.DeviceId, cancellationToken);
        if (dispositivo is null || !dispositivo.Ativo)
            return ResponseDefault<BaterPontoMobileCommandResult>.Forbidden(
                $"Dispositivo {request.DeviceId} não registrado ou revogado.");

        // 3. Timestamp do device ± 5min do servidor
        var agora = DateTime.UtcNow;
        var diff = (agora - request.TimestampLocal).Duration();
        if (diff > ToleranciaRelogio)
            return ResponseDefault<BaterPontoMobileCommandResult>.BadRequest(
                Core.Response.Erros.Error.Validation(
                    $"timestampLocal divergente do servidor em {diff.TotalMinutes:N0}min (limite: 5min)."));

        // 4. hashBatida confere (mesma fórmula do HashHelpers no app)
        var funcionarioId = dispositivo.FuncionarioId ?? userId.Value;
        var hashEsperado = CalcularHashBatida(funcionarioId, request.TimestampLocal, request.Tipo?.ToString(), request.DeviceId);
        if (!string.Equals(request.HashBatida, hashEsperado, StringComparison.OrdinalIgnoreCase))
            return ResponseDefault<BaterPontoMobileCommandResult>.BadRequest(
                Core.Response.Erros.Error.Validation("hashBatida inválido."));

        // 5. Upload foto (stub: TODO integrar IS3StorageService quando existir)
        string? fotoUrl = null;
        if (request.FotoBytes is { Length: > 0 })
        {
            var nome = $"{_tenantContext.TenantId:N}/{funcionarioId:N}/ponto/{agora:yyyyMM}/{Guid.NewGuid():N}.jpg";
            // Placeholder: em prod, faz upload para GED/S3 e armazena URL pública/assinada.
            fotoUrl = $"s3://atena-ponto/{nome}";
        }

        // 6. Cria MarcacaoPonto com origem=MobileApp e cadeia de hash igual ao bater web
        var ultima = await _repo.GetUltimaPorFuncionarioAsync(funcionarioId, cancellationToken);
        var origem = OrigemMarcacao.MobileApp;
        var tipo = request.Tipo ?? InferirProximoTipo(ultima);
        var hashIntegridade = MarcacaoPontoIntegridade.Calcular(
            funcionarioId, agora, tipo, origem, ultima?.HashIntegridade);

        var marcacao = new MarcacaoEntity
        {
            TenantId = _tenantContext.TenantId,
            FuncionarioId = funcionarioId,
            Tipo = tipo,
            DataHora = agora,
            Origem = origem,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            DeviceId = request.DeviceId,
            FotoUrl = fotoUrl,
            HashAnterior = ultima?.HashIntegridade,
            HashIntegridade = hashIntegridade,
            Status = StatusMarcacao.Valida,
            CreatedBy = userId,
        };
        await _repo.AddAsync(marcacao, cancellationToken);

        // 7. Atualiza ultimo_acesso do dispositivo
        await _dispositivos.RegistrarUltimoAcessoAsync(dispositivo.Id, cancellationToken);

        return ResponseDefault<BaterPontoMobileCommandResult>.Created(
            new BaterPontoMobileCommandResult(marcacao.Id, marcacao.DataHora, marcacao.Tipo,
                marcacao.HashIntegridade, fotoUrl));
    }

    private static string CalcularHashBatida(Guid funcionarioId, DateTime timestamp, string? tipo, string deviceId)
    {
        var payload = string.Join("|",
            funcionarioId.ToString("D"),
            timestamp.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            tipo ?? string.Empty,
            deviceId);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private static TipoMarcacao InferirProximoTipo(MarcacaoEntity? ultima)
    {
        if (ultima is null || ultima.DataHora.Date != DateTime.UtcNow.Date) return TipoMarcacao.Entrada;
        return ultima.Tipo switch
        {
            TipoMarcacao.Entrada => TipoMarcacao.SaidaAlmoco,
            TipoMarcacao.SaidaAlmoco => TipoMarcacao.VoltaAlmoco,
            TipoMarcacao.VoltaAlmoco => TipoMarcacao.Saida,
            TipoMarcacao.Saida => TipoMarcacao.Entrada,
            _ => TipoMarcacao.Entrada,
        };
    }
}
