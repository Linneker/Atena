using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using MovimentoEntity = Acme.Sistemas.Domain.Entities.Rh.MovimentoBancoHoras;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.CompensarHoras;

/// <summary>
/// Compensa horas do banco — gera MovimentoBancoHoras com origem=Compensacao e
/// minutos negativos (consome saldo positivo). Não emite cheque-folha; só registra.
/// </summary>
public sealed class CompensarHorasCommandHandler
    : IRequestHandler<CompensarHorasCommand, ResponseDefault<CompensarHorasCommandResult>>
{
    private readonly IMovimentoBancoHorasRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CompensarHorasCommandHandler(IMovimentoBancoHorasRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CompensarHorasCommandResult>> Handle(
        CompensarHorasCommand request, CancellationToken cancellationToken)
    {
        var competencia = request.Data.ToString("yyyy-MM");
        var mov = new MovimentoEntity
        {
            TenantId = _tenantContext.TenantId,
            FuncionarioId = request.FuncionarioId,
            Data = request.Data,
            Origem = OrigemMovimentoBancoHoras.Compensacao,
            Minutos = -Math.Abs(request.Minutos),  // sempre negativo
            Competencia = competencia,
            Observacao = request.Motivo,
            CreatedBy = _tenantContext.UserId,
        };
        await _repo.AddAsync(mov, cancellationToken);

        return ResponseDefault<CompensarHorasCommandResult>.Created(
            new CompensarHorasCommandResult(mov.Id, mov.Minutos));
    }
}
