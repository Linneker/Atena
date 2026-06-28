using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using ReceitaEntity = Acme.Sistemas.Domain.Entities.Financeiro.Receita;

namespace Acme.Sistemas.Services.V1.Receita.Command.GerarRecorrencias;

public sealed class GerarRecorrenciasReceitaCommandHandler
    : IRequestHandler<GerarRecorrenciasReceitaCommand, ResponseDefault<GerarRecorrenciasReceitaCommandResult>>
{
    private readonly IReceitaRepository _receitas;
    private readonly ITenantContext _tenantContext;

    public GerarRecorrenciasReceitaCommandHandler(IReceitaRepository receitas, ITenantContext tenantContext)
    {
        _receitas = receitas;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<GerarRecorrenciasReceitaCommandResult>> Handle(
        GerarRecorrenciasReceitaCommand request, CancellationToken cancellationToken)
    {
        var todas = await _receitas.ListByFiltroAsync(null, null, null, null, null, 0, 1000, cancellationToken);
        var fixas = todas.Where(r => r.ReceitaFixa).ToList();

        int geradas = 0, ignoradas = 0;
        var hoje = DateTime.UtcNow.Date;

        foreach (var template in fixas)
        {
            for (int offset = 1; offset <= request.Meses; offset++)
            {
                var dataAlvo = AjustarParaMes(template.DataPrevistaRecebimento, hoje.Year, hoje.Month + offset);

                var primeiroDoMes = new DateTime(dataAlvo.Year, dataAlvo.Month, 1);
                var ultimoDoMes = primeiroDoMes.AddMonths(1).AddDays(-1);
                var existentes = await _receitas.ListByFiltroAsync(
                    null, primeiroDoMes, ultimoDoMes, null, null, 0, 200, cancellationToken);
                if (existentes.Any(r => r.Nome == template.Nome))
                {
                    ignoradas++;
                    continue;
                }

                var nova = new ReceitaEntity
                {
                    TenantId = _tenantContext.TenantId,
                    Nome = template.Nome,
                    Descricao = template.Descricao,
                    Categoria = template.Categoria,
                    Valor = template.Valor,
                    ReceitaFixa = false,
                    DataPrevistaRecebimento = dataAlvo,
                    CentroDeCustoId = template.CentroDeCustoId,
                    ClienteId = template.ClienteId,
                    OrigemReceitaId = template.Id,
                    StatusRecebimento = StatusPagamento.Pendente,
                    CreatedBy = _tenantContext.UserId
                };
                await _receitas.AddAsync(nova, cancellationToken);
                geradas++;
            }
        }

        return ResponseDefault<GerarRecorrenciasReceitaCommandResult>.Ok(
            new GerarRecorrenciasReceitaCommandResult(geradas, ignoradas));
    }

    private static DateTime AjustarParaMes(DateTime referencia, int ano, int mes)
    {
        while (mes > 12) { mes -= 12; ano++; }
        var dia = Math.Min(referencia.Day, DateTime.DaysInMonth(ano, mes));
        return new DateTime(ano, mes, dia, referencia.Hour, referencia.Minute, referencia.Second, referencia.Kind);
    }
}
