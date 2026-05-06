using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Divida.Command.AlterarDivida;

public sealed class AlterarDividaCommandHandler
    : IRequestHandler<AlterarDividaCommand, ResponseDefault<AlterarDividaCommandResult>>
{
    private readonly IDividaRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AlterarDividaCommandHandler(IDividaRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarDividaCommandResult>> Handle(AlterarDividaCommand request, CancellationToken cancellationToken)
    {
        var divida = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (divida is null)
            return ResponseDefault<AlterarDividaCommandResult>.NotFound("Dívida não encontrada.");

        if (divida.Status == StatusConta.Pago)
            return ResponseDefault<AlterarDividaCommandResult>.Conflict("Dívida já quitada não pode ser alterada.");

        divida.Credor = request.Credor;
        divida.Descricao = request.Descricao;
        divida.ValorOriginal = request.ValorOriginal;
        divida.TaxaJurosMensal = request.TaxaJurosMensal;
        divida.DataInicio = request.DataInicio;
        divida.DataFim = request.DataFim;
        divida.NumeroParcelas = request.NumeroParcelas;
        divida.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(divida, cancellationToken);
        return ResponseDefault<AlterarDividaCommandResult>.Ok(new AlterarDividaCommandResult(divida.Id));
    }
}
