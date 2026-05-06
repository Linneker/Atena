using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Command.ExcluirCentroDeCusto;

public sealed class ExcluirCentroDeCustoCommandHandler : IRequestHandler<ExcluirCentroDeCustoCommand, ResponseDefault>
{
    private readonly ICentroDeCustoRepository _repo;

    public ExcluirCentroDeCustoCommandHandler(ICentroDeCustoRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault> Handle(ExcluirCentroDeCustoCommand request, CancellationToken cancellationToken)
    {
        var centro = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (centro is null)
            return ResponseDefault.BadRequest(Error.NotFound("Centro de custo não encontrado."));

        var vinculos = await _repo.CountVinculosAsync(centro.Id, cancellationToken);
        if (vinculos > 0)
            return ResponseDefault.BadRequest(Error.Conflict(
                $"Centro de custo possui {vinculos} lançamentos vinculados (despesas/receitas) e não pode ser excluído. Marque como inativo."));

        await _repo.DeleteAsync(centro.Id, cancellationToken);
        return ResponseDefault.NoContent();
    }
}
