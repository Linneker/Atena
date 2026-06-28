using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using DispositivoEntity = Acme.Sistemas.Domain.Entities.Rh.DispositivoMobile;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.RegistrarDispositivo;

/// <summary>
/// Idempotente por (tenant, usuario, deviceId). Se já existe, atualiza pushToken,
/// chavePublicaLocal, versão e marca como ativo. Se não existe, cria novo.
/// </summary>
public sealed class RegistrarDispositivoCommandHandler
    : IRequestHandler<RegistrarDispositivoCommand, ResponseDefault<RegistrarDispositivoCommandResult>>
{
    private readonly IDispositivoMobileRepository _repo;
    private readonly ITenantContext _tenantContext;

    public RegistrarDispositivoCommandHandler(IDispositivoMobileRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<RegistrarDispositivoCommandResult>> Handle(
        RegistrarDispositivoCommand request, CancellationToken cancellationToken)
    {
        var userId = _tenantContext.UserId
            ?? throw new InvalidOperationException("UserId obrigatório para registrar dispositivo.");

        var existente = await _repo.GetByDeviceIdAsync(userId, request.DeviceId, cancellationToken);
        if (existente is not null)
        {
            existente.Modelo = request.Modelo;
            existente.OsVersion = request.OsVersion;
            existente.AppVersion = request.AppVersion;
            existente.PushToken = request.PushToken;
            existente.ChavePublicaLocal = request.ChavePublicaLocal ?? existente.ChavePublicaLocal;
            existente.Ativo = true;
            existente.UltimoAcesso = DateTime.UtcNow;
            existente.UpdatedBy = userId;
            await _repo.UpdateAsync(existente, cancellationToken);
            return ResponseDefault<RegistrarDispositivoCommandResult>.Ok(
                new RegistrarDispositivoCommandResult(existente.Id, JaExistia: true));
        }

        var novo = new DispositivoEntity
        {
            TenantId = _tenantContext.TenantId,
            UsuarioId = userId,
            FuncionarioId = userId, // 1:1 com user (W4 separa caso ≠)
            DeviceId = request.DeviceId,
            Plataforma = request.Plataforma,
            Modelo = request.Modelo,
            OsVersion = request.OsVersion,
            AppVersion = request.AppVersion,
            PushToken = request.PushToken,
            ChavePublicaLocal = request.ChavePublicaLocal,
            Ativo = true,
            RegistradoEm = DateTime.UtcNow,
            UltimoAcesso = DateTime.UtcNow,
            CreatedBy = userId,
        };
        await _repo.AddAsync(novo, cancellationToken);

        return ResponseDefault<RegistrarDispositivoCommandResult>.Created(
            new RegistrarDispositivoCommandResult(novo.Id, JaExistia: false));
    }
}
