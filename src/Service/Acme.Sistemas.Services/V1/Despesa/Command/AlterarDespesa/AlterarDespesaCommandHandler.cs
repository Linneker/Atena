using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Despesa.Command.AlterarDespesa;

public sealed class AlterarDespesaCommandHandler
    : IRequestHandler<AlterarDespesaCommand, ResponseDefault<AlterarDespesaCommandResult>>
{
    private readonly IDespesaRepository _despesas;
    private readonly ITenantContext _tenantContext;

    public AlterarDespesaCommandHandler(IDespesaRepository despesas, ITenantContext tenantContext)
    {
        _despesas = despesas;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarDespesaCommandResult>> Handle(
        AlterarDespesaCommand request,
        CancellationToken cancellationToken)
    {
        var despesa = await _despesas.GetByIdAsync(request.Id, cancellationToken);
        if (despesa is null)
        {
            return ResponseDefault<AlterarDespesaCommandResult>.NotFound("Despesa não encontrada.");
        }

        if (despesa.StatusPagamento == StatusPagamento.Pago)
        {
            return ResponseDefault<AlterarDespesaCommandResult>.Conflict(
                "Despesa já paga não pode ser alterada. Cancele e crie uma nova.");
        }

        despesa.Nome = request.Nome;
        despesa.Descricao = request.Descricao;
        despesa.Categoria = request.Categoria;
        despesa.Valor = request.Valor;
        despesa.DespesaFixa = request.DespesaFixa;
        despesa.DataVencimento = request.DataVencimento;
        despesa.CompetenciaId = request.CompetenciaId;
        despesa.CentroDeCustoId = request.CentroDeCustoId;
        despesa.FornecedorId = request.FornecedorId;
        despesa.UpdatedBy = _tenantContext.UserId;

        await _despesas.UpdateAsync(despesa, cancellationToken);

        return ResponseDefault<AlterarDespesaCommandResult>.Ok(new AlterarDespesaCommandResult(despesa.Id));
    }
}
