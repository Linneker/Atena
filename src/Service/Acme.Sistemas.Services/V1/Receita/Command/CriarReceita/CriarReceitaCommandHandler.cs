using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Receita.Command.CriarReceita;

public sealed class CriarReceitaCommandHandler
    : IRequestHandler<CriarReceitaCommand, ResponseDefault<CriarReceitaCommandResult>>
{
    private readonly IReceitaRepository _receitas;
    private readonly ITenantContext _tenantContext;

    public CriarReceitaCommandHandler(IReceitaRepository receitas, ITenantContext tenantContext)
    {
        _receitas = receitas;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarReceitaCommandResult>> Handle(
        CriarReceitaCommand request,
        CancellationToken cancellationToken)
    {
        var receita = new Domain.Entities.Financeiro.Receita
        {
            TenantId = _tenantContext.TenantId,
            Nome = request.Nome,
            Descricao = request.Descricao,
            Categoria = request.Categoria,
            Valor = request.Valor,
            ReceitaFixa = request.ReceitaFixa,
            DataPrevistaRecebimento = request.DataPrevistaRecebimento,
            CompetenciaId = request.CompetenciaId,
            CentroDeCustoId = request.CentroDeCustoId,
            ClienteId = request.ClienteId,
            OrigemVendaId = request.OrigemVendaId,
            StatusRecebimento = StatusPagamento.Pendente,
            CreatedBy = _tenantContext.UserId
        };

        await _receitas.AddAsync(receita, cancellationToken);

        return ResponseDefault<CriarReceitaCommandResult>.Created(
            new CriarReceitaCommandResult(receita.Id, receita.Nome, receita.Valor, receita.DataPrevistaRecebimento));
    }
}
