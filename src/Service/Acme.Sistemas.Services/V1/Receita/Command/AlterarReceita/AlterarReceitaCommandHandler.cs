using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Receita.Command.AlterarReceita;

public sealed class AlterarReceitaCommandHandler
    : IRequestHandler<AlterarReceitaCommand, ResponseDefault<AlterarReceitaCommandResult>>
{
    private readonly IReceitaRepository _receitas;
    private readonly ITenantContext _tenantContext;

    public AlterarReceitaCommandHandler(IReceitaRepository receitas, ITenantContext tenantContext)
    {
        _receitas = receitas;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarReceitaCommandResult>> Handle(
        AlterarReceitaCommand request,
        CancellationToken cancellationToken)
    {
        var receita = await _receitas.GetByIdAsync(request.Id, cancellationToken);
        if (receita is null)
        {
            return ResponseDefault<AlterarReceitaCommandResult>.NotFound("Receita não encontrada.");
        }

        if (receita.StatusRecebimento == StatusPagamento.Pago)
        {
            return ResponseDefault<AlterarReceitaCommandResult>.Conflict(
                "Receita já recebida não pode ser alterada. Cancele e crie uma nova.");
        }

        receita.Nome = request.Nome;
        receita.Descricao = request.Descricao;
        receita.Categoria = request.Categoria;
        receita.Valor = request.Valor;
        receita.ReceitaFixa = request.ReceitaFixa;
        receita.DataPrevistaRecebimento = request.DataPrevistaRecebimento;
        receita.CompetenciaId = request.CompetenciaId;
        receita.CentroDeCustoId = request.CentroDeCustoId;
        receita.ClienteId = request.ClienteId;
        receita.OrigemVendaId = request.OrigemVendaId;
        receita.UpdatedBy = _tenantContext.UserId;

        await _receitas.UpdateAsync(receita, cancellationToken);

        return ResponseDefault<AlterarReceitaCommandResult>.Ok(new AlterarReceitaCommandResult(receita.Id));
    }
}
