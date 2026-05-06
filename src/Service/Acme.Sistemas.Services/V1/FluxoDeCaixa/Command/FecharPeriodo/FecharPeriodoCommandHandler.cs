using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.FluxoDeCaixa.Command.FecharPeriodo;

public sealed class FecharPeriodoCommandHandler
    : IRequestHandler<FecharPeriodoCommand, ResponseDefault<FecharPeriodoCommandResult>>
{
    private readonly IFechamentoPeriodoRepository _fechamentos;
    private readonly IDespesaRepository _despesas;
    private readonly IReceitaRepository _receitas;
    private readonly ITenantContext _tenantContext;

    public FecharPeriodoCommandHandler(
        IFechamentoPeriodoRepository fechamentos,
        IDespesaRepository despesas,
        IReceitaRepository receitas,
        ITenantContext tenantContext)
    {
        _fechamentos = fechamentos;
        _despesas = despesas;
        _receitas = receitas;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<FecharPeriodoCommandResult>> Handle(
        FecharPeriodoCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await _fechamentos.GetByPeriodoAsync(request.Ano, request.Mes, cancellationToken);
        if (existing is not null)
        {
            return ResponseDefault<FecharPeriodoCommandResult>.Conflict(
                $"Período {request.Mes:00}/{request.Ano} já está fechado.");
        }

        var inicio = new DateTime(request.Ano, request.Mes, 1, 0, 0, 0, DateTimeKind.Utc);
        var fim = inicio.AddMonths(1).AddTicks(-1);

        var totalReceitas = await _receitas.SumByPeriodoAsync(inicio, fim, somenteRecebidas: false, cancellationToken);
        var totalDespesas = await _despesas.SumByPeriodoAsync(inicio, fim, somenteBaixadas: false, cancellationToken);

        var fechamento = new FechamentoPeriodo
        {
            TenantId = _tenantContext.TenantId,
            Ano = request.Ano,
            Mes = request.Mes,
            FechadoEm = DateTime.UtcNow,
            FechadoPor = _tenantContext.UserId,
            TotalReceitas = totalReceitas,
            TotalDespesas = totalDespesas,
            Resultado = totalReceitas - totalDespesas,
            Observacao = request.Observacao,
            CreatedBy = _tenantContext.UserId
        };

        await _fechamentos.AddAsync(fechamento, cancellationToken);

        return ResponseDefault<FecharPeriodoCommandResult>.Created(new FecharPeriodoCommandResult(
            fechamento.Id, fechamento.Ano, fechamento.Mes,
            fechamento.TotalReceitas, fechamento.TotalDespesas, fechamento.Resultado,
            fechamento.FechadoEm));
    }
}
