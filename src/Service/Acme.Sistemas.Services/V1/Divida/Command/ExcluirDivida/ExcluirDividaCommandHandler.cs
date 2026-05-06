using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Divida.Command.ExcluirDivida;

public sealed class ExcluirDividaCommandHandler : IRequestHandler<ExcluirDividaCommand, ResponseDefault>
{
    private readonly IDividaRepository _repo;

    public ExcluirDividaCommandHandler(IDividaRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault> Handle(ExcluirDividaCommand request, CancellationToken cancellationToken)
    {
        var divida = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (divida is null)
            return ResponseDefault.BadRequest(Error.NotFound("Dívida não encontrada."));

        if (divida.Status == StatusConta.Pago || divida.ValorPago > 0)
            return ResponseDefault.BadRequest(Error.Conflict("Dívida com pagamentos registrados não pode ser excluída."));

        await _repo.DeleteAsync(request.Id, cancellationToken);
        return ResponseDefault.NoContent();
    }
}
