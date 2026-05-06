using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Vendas;
using Acme.Sistemas.Domain.Interfaces.Repository;
using OrcamentoEntity = Acme.Sistemas.Domain.Entities.Vendas.Orcamento;

namespace Acme.Sistemas.Services.V1.Orcamento.Command.CriarOrcamento;

public sealed class CriarOrcamentoCommandHandler
    : IRequestHandler<CriarOrcamentoCommand, ResponseDefault<CriarOrcamentoCommandResult>>
{
    private readonly IOrcamentoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarOrcamentoCommandHandler(IOrcamentoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarOrcamentoCommandResult>> Handle(CriarOrcamentoCommand request, CancellationToken cancellationToken)
    {
        var seq = await _repo.NextNumeroAsync(cancellationToken);
        var numero = $"ORC{DateTime.UtcNow:yyyyMM}{seq:D5}";
        var subtotal = request.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);
        var valor = subtotal * (1 - (request.DescontoPercentual ?? 0) / 100m);

        var orcamento = new OrcamentoEntity
        {
            TenantId = _tenantContext.TenantId,
            Numero = numero,
            ClienteId = request.ClienteId,
            VendedorId = request.VendedorId,
            DataEmissao = DateTime.UtcNow,
            DataValidade = request.DataValidade,
            ValorTotal = valor,
            DescontoPercentual = request.DescontoPercentual,
            Status = StatusOrcamento.Rascunho,
            Observacao = request.Observacao,
            CreatedBy = _tenantContext.UserId
        };

        await _repo.AddAsync(orcamento, cancellationToken);
        await _repo.AddItensAsync(request.Itens.Select(i => new OrcamentoItem
        {
            TenantId = _tenantContext.TenantId,
            OrcamentoId = orcamento.Id,
            ProdutoId = i.ProdutoId,
            Quantidade = i.Quantidade,
            PrecoUnitario = i.PrecoUnitario,
            CreatedBy = _tenantContext.UserId
        }), cancellationToken);

        return ResponseDefault<CriarOrcamentoCommandResult>.Created(
            new CriarOrcamentoCommandResult(orcamento.Id, numero, valor));
    }
}
