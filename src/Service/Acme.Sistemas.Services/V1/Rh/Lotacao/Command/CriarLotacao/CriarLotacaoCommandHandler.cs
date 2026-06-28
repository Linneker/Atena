using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using LotacaoEntity = Acme.Sistemas.Domain.Entities.Rh.Lotacao;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Command.CriarLotacao;

public sealed class CriarLotacaoCommandHandler
    : IRequestHandler<CriarLotacaoCommand, ResponseDefault<CriarLotacaoCommandResult>>
{
    private readonly ILotacaoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarLotacaoCommandHandler(ILotacaoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarLotacaoCommandResult>> Handle(
        CriarLotacaoCommand request, CancellationToken cancellationToken)
    {
        var existente = await _repo.GetByNomeAsync(request.Nome, cancellationToken);
        if (existente is not null)
            return ResponseDefault<CriarLotacaoCommandResult>.Conflict(
                $"Já existe uma lotação com o nome '{request.Nome}'.");

        var lotacao = new LotacaoEntity
        {
            TenantId = _tenantContext.TenantId,
            Nome = request.Nome,
            EmpresaId = request.EmpresaId,
            Cnpj = request.Cnpj,
            EnderecoJson = request.EnderecoJson,
            Ativo = true,
            CreatedBy = _tenantContext.UserId,
        };

        await _repo.AddAsync(lotacao, cancellationToken);

        return ResponseDefault<CriarLotacaoCommandResult>.Created(
            new CriarLotacaoCommandResult(lotacao.Id, lotacao.Nome));
    }
}
