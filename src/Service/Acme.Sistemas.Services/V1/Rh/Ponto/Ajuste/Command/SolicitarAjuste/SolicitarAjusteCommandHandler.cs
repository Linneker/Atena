using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using AjusteEntity = Acme.Sistemas.Domain.Entities.Rh.AjustePonto;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.SolicitarAjuste;

public sealed class SolicitarAjusteCommandHandler
    : IRequestHandler<SolicitarAjusteCommand, ResponseDefault<SolicitarAjusteCommandResult>>
{
    private readonly IAjustePontoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public SolicitarAjusteCommandHandler(IAjustePontoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<SolicitarAjusteCommandResult>> Handle(
        SolicitarAjusteCommand request, CancellationToken cancellationToken)
    {
        var userId = _tenantContext.UserId;
        if (userId is null)
            return ResponseDefault<SolicitarAjusteCommandResult>.Forbidden("Usuário não autenticado.");

        var ajuste = new AjusteEntity
        {
            TenantId = _tenantContext.TenantId,
            FuncionarioId = userId.Value,  // funcionário == user logado para self-service
            MarcacaoOriginalId = request.MarcacaoOriginalId,
            TipoAjuste = request.TipoAjuste,
            DataHoraProposta = request.DataHoraProposta,
            TipoMarcacaoProposta = request.TipoMarcacaoProposta,
            Motivo = request.Motivo,
            AnexoUrl = request.AnexoUrl,
            Status = StatusAjuste.Pendente,
            CreatedBy = userId,
        };
        await _repo.AddAsync(ajuste, cancellationToken);

        return ResponseDefault<SolicitarAjusteCommandResult>.Created(new SolicitarAjusteCommandResult(ajuste.Id));
    }
}
