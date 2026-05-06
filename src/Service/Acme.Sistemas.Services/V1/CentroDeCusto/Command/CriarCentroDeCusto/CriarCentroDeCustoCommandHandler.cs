using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using CentroDeCustoEntity = Acme.Sistemas.Domain.Entities.Financeiro.CentroDeCusto;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Command.CriarCentroDeCusto;

public sealed class CriarCentroDeCustoCommandHandler
    : IRequestHandler<CriarCentroDeCustoCommand, ResponseDefault<CriarCentroDeCustoCommandResult>>
{
    private readonly ICentroDeCustoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarCentroDeCustoCommandHandler(ICentroDeCustoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarCentroDeCustoCommandResult>> Handle(CriarCentroDeCustoCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repo.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existing is not null)
            return ResponseDefault<CriarCentroDeCustoCommandResult>.Conflict(
                $"Já existe centro de custo com o código {request.Codigo}.");

        var centro = new CentroDeCustoEntity
        {
            TenantId = _tenantContext.TenantId,
            Codigo = request.Codigo,
            Nome = request.Nome,
            Descricao = request.Descricao,
            ResponsavelId = request.ResponsavelId,
            Ativo = true,
            CreatedBy = _tenantContext.UserId
        };

        await _repo.AddAsync(centro, cancellationToken);
        return ResponseDefault<CriarCentroDeCustoCommandResult>.Created(
            new CriarCentroDeCustoCommandResult(centro.Id, centro.Codigo, centro.Nome));
    }
}
