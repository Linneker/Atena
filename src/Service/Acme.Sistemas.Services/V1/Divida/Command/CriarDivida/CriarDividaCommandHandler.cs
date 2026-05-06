using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Divida.Command.CriarDivida;

public sealed class CriarDividaCommandHandler
    : IRequestHandler<CriarDividaCommand, ResponseDefault<CriarDividaCommandResult>>
{
    private readonly IDividaRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarDividaCommandHandler(IDividaRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarDividaCommandResult>> Handle(CriarDividaCommand request, CancellationToken cancellationToken)
    {
        var divida = new Domain.Entities.Financeiro.Divida
        {
            TenantId = _tenantContext.TenantId,
            Credor = request.Credor,
            Descricao = request.Descricao,
            ValorOriginal = request.ValorOriginal,
            TaxaJurosMensal = request.TaxaJurosMensal,
            DataInicio = request.DataInicio,
            DataFim = request.DataFim,
            NumeroParcelas = request.NumeroParcelas,
            Status = StatusConta.Pendente,
            CreatedBy = _tenantContext.UserId
        };

        await _repo.AddAsync(divida, cancellationToken);

        return ResponseDefault<CriarDividaCommandResult>.Created(
            new CriarDividaCommandResult(divida.Id, divida.Credor, divida.ValorOriginal));
    }
}
