using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using MovimentoEntity = Acme.Sistemas.Domain.Entities.Rh.MovimentoBancoHoras;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.PagarSaldo;

/// <summary>
/// Paga saldo positivo do banco de horas — gera MovimentoBancoHoras com origem=Pagamento
/// (minutos negativos) e registra pendência para folha (W6) lançar como rubrica.
/// </summary>
public sealed class PagarSaldoCommandHandler
    : IRequestHandler<PagarSaldoCommand, ResponseDefault<PagarSaldoCommandResult>>
{
    private readonly IMovimentoBancoHorasRepository _repo;
    private readonly ITenantContext _tenantContext;

    public PagarSaldoCommandHandler(IMovimentoBancoHorasRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<PagarSaldoCommandResult>> Handle(
        PagarSaldoCommand request, CancellationToken cancellationToken)
    {
        var data = DateOnly.ParseExact(request.Competencia + "-01", "yyyy-MM-dd");
        var mov = new MovimentoEntity
        {
            TenantId = _tenantContext.TenantId,
            FuncionarioId = request.FuncionarioId,
            Data = data,
            Origem = OrigemMovimentoBancoHoras.Pagamento,
            Minutos = -Math.Abs(request.Minutos),
            Competencia = request.Competencia,
            Observacao = $"Pago via folha competência {request.Competencia}",
            CreatedBy = _tenantContext.UserId,
        };
        await _repo.AddAsync(mov, cancellationToken);

        // PendenciaFolha é apenas indicador para W6 buscar (via query)
        var pendenciaRef = $"PendFolha:{request.FuncionarioId}:{request.Competencia}";

        return ResponseDefault<PagarSaldoCommandResult>.Created(
            new PagarSaldoCommandResult(mov.Id, mov.Minutos, request.Competencia, pendenciaRef));
    }
}
