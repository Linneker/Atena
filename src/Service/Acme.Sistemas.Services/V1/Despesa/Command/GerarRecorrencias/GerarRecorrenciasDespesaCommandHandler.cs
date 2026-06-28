using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using DespesaEntity = Acme.Sistemas.Domain.Entities.Financeiro.Despesa;

namespace Acme.Sistemas.Services.V1.Despesa.Command.GerarRecorrencias;

public sealed class GerarRecorrenciasDespesaCommandHandler
    : IRequestHandler<GerarRecorrenciasDespesaCommand, ResponseDefault<GerarRecorrenciasDespesaCommandResult>>
{
    private readonly IDespesaRepository _despesas;
    private readonly ITenantContext _tenantContext;

    public GerarRecorrenciasDespesaCommandHandler(IDespesaRepository despesas, ITenantContext tenantContext)
    {
        _despesas = despesas;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<GerarRecorrenciasDespesaCommandResult>> Handle(
        GerarRecorrenciasDespesaCommand request, CancellationToken cancellationToken)
    {
        // Pega todas as despesas (até 1000) do tenant. Filtra fixas em memória.
        var todas = await _despesas.ListByFiltroAsync(null, null, null, null, null, 0, 1000, cancellationToken);
        var fixas = todas.Where(d => d.DespesaFixa).ToList();

        int geradas = 0, ignoradas = 0;
        var hoje = DateTime.UtcNow.Date;

        foreach (var template in fixas)
        {
            for (int offset = 1; offset <= request.Meses; offset++)
            {
                var dataAlvo = AjustarParaMes(template.DataVencimento, hoje.Year, hoje.Month + offset);

                // já existe entry no ano-mês alvo com o mesmo Nome?
                var primeiroDoMes = new DateTime(dataAlvo.Year, dataAlvo.Month, 1);
                var ultimoDoMes = primeiroDoMes.AddMonths(1).AddDays(-1);
                var existentes = await _despesas.ListByFiltroAsync(
                    null, primeiroDoMes, ultimoDoMes, null, null, 0, 200, cancellationToken);
                if (existentes.Any(d => d.Nome == template.Nome))
                {
                    ignoradas++;
                    continue;
                }

                var nova = new DespesaEntity
                {
                    TenantId = _tenantContext.TenantId,
                    Nome = template.Nome,
                    Descricao = template.Descricao,
                    Categoria = template.Categoria,
                    Valor = template.Valor,
                    DespesaFixa = false, // a instância gerada não é template
                    DataVencimento = dataAlvo,
                    CentroDeCustoId = template.CentroDeCustoId,
                    FornecedorId = template.FornecedorId,
                    OrigemDespesaId = template.Id,
                    StatusPagamento = StatusPagamento.Pendente,
                    CreatedBy = _tenantContext.UserId
                };
                await _despesas.AddAsync(nova, cancellationToken);
                geradas++;
            }
        }

        return ResponseDefault<GerarRecorrenciasDespesaCommandResult>.Ok(
            new GerarRecorrenciasDespesaCommandResult(geradas, ignoradas));
    }

    /// <summary>
    /// Mantém o "dia" do template ajustando para meses futuros, lidando com mês destino sem aquele dia
    /// (ex: dia 31 indo para fevereiro → último dia do mês).
    /// </summary>
    private static DateTime AjustarParaMes(DateTime referencia, int ano, int mes)
    {
        while (mes > 12) { mes -= 12; ano++; }
        var dia = Math.Min(referencia.Day, DateTime.DaysInMonth(ano, mes));
        return new DateTime(ano, mes, dia, referencia.Hour, referencia.Minute, referencia.Second, referencia.Kind);
    }
}
