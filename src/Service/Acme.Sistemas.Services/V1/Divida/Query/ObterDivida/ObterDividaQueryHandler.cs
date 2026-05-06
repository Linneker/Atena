using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Divida.Query.ObterDivida;

public sealed class ObterDividaQueryHandler
    : IRequestHandler<ObterDividaQuery, ResponseDefault<ObterDividaQueryResult>>
{
    private readonly IDividaRepository _repo;

    public ObterDividaQueryHandler(IDividaRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ObterDividaQueryResult>> Handle(ObterDividaQuery request, CancellationToken cancellationToken)
    {
        var d = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (d is null)
            return ResponseDefault<ObterDividaQueryResult>.NotFound("Dívida não encontrada.");

        return ResponseDefault<ObterDividaQueryResult>.Ok(new ObterDividaQueryResult(
            d.Id, d.Credor, d.Descricao,
            d.ValorOriginal, d.ValorPago, d.Saldo,
            d.TaxaJurosMensal, d.DataInicio, d.DataFim,
            d.NumeroParcelas, d.Status, d.CreatedAt));
    }
}
