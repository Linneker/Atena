using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Despesa.Command.CriarDespesa;

public sealed class CriarDespesaCommandHandler
    : IRequestHandler<CriarDespesaCommand, ResponseDefault<CriarDespesaCommandResult>>
{
    private readonly IDespesaRepository _despesas;
    private readonly ITenantContext _tenantContext;

    public CriarDespesaCommandHandler(IDespesaRepository despesas, ITenantContext tenantContext)
    {
        _despesas = despesas;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarDespesaCommandResult>> Handle(
        CriarDespesaCommand request,
        CancellationToken cancellationToken)
    {
        var despesa = new Domain.Entities.Financeiro.Despesa
        {
            TenantId = _tenantContext.TenantId,
            Nome = request.Nome,
            Descricao = request.Descricao,
            Categoria = request.Categoria,
            Valor = request.Valor,
            DespesaFixa = request.DespesaFixa,
            DataVencimento = request.DataVencimento,
            CompetenciaId = request.CompetenciaId,
            CentroDeCustoId = request.CentroDeCustoId,
            FornecedorId = request.FornecedorId,
            StatusPagamento = StatusPagamento.Pendente,
            CreatedBy = _tenantContext.UserId
        };

        await _despesas.AddAsync(despesa, cancellationToken);

        return ResponseDefault<CriarDespesaCommandResult>.Created(
            new CriarDespesaCommandResult(despesa.Id, despesa.Nome, despesa.Valor, despesa.DataVencimento));
    }
}
