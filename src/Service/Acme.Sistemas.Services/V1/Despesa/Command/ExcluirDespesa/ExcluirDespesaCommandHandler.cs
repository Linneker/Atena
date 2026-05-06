using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Despesa.Command.ExcluirDespesa;

public sealed class ExcluirDespesaCommandHandler : IRequestHandler<ExcluirDespesaCommand, ResponseDefault>
{
    private readonly IDespesaRepository _despesas;
    private readonly ITenantContext _tenantContext;

    public ExcluirDespesaCommandHandler(IDespesaRepository despesas, ITenantContext tenantContext)
    {
        _despesas = despesas;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault> Handle(ExcluirDespesaCommand request, CancellationToken cancellationToken)
    {
        var despesa = await _despesas.GetByIdAsync(request.Id, cancellationToken);
        if (despesa is null)
            return ResponseDefault.BadRequest(Error.NotFound("Despesa não encontrada."));

        if (despesa.StatusPagamento == StatusPagamento.Pago)
            return ResponseDefault.BadRequest(Error.Conflict(
                "Não é possível excluir uma despesa já paga. Cancele o lançamento de baixa primeiro."));

        var deletedBy = _tenantContext.UserId ?? Guid.Empty;
        await _despesas.DeleteAsync(request.Id, cancellationToken);

        return ResponseDefault.NoContent();
    }
}
