using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using BeneficioEntity = Acme.Sistemas.Domain.Entities.Rh.BeneficioCatalogo;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Command.CriarBeneficioCatalogo;

public sealed class CriarBeneficioCatalogoCommandHandler
    : IRequestHandler<CriarBeneficioCatalogoCommand, ResponseDefault<CriarBeneficioCatalogoCommandResult>>
{
    private readonly IBeneficioCatalogoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarBeneficioCatalogoCommandHandler(IBeneficioCatalogoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarBeneficioCatalogoCommandResult>> Handle(
        CriarBeneficioCatalogoCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.Codigo))
        {
            var existente = await _repo.GetByCodigoAsync(request.Codigo, cancellationToken);
            if (existente is not null)
                return ResponseDefault<CriarBeneficioCatalogoCommandResult>.Conflict(
                    $"Já existe um benefício com o código '{request.Codigo}'.");
        }

        var benef = new BeneficioEntity
        {
            TenantId = _tenantContext.TenantId,
            Codigo = request.Codigo,
            Descricao = request.Descricao,
            Tipo = request.Tipo,
            DescontoFuncionarioPct = request.DescontoFuncionarioPct,
            CustoEmpresaPadrao = request.CustoEmpresaPadrao,
            NaturezaRubricaEsocial = request.NaturezaRubricaEsocial,
            Ativo = true,
            CreatedBy = _tenantContext.UserId,
        };

        await _repo.AddAsync(benef, cancellationToken);

        return ResponseDefault<CriarBeneficioCatalogoCommandResult>.Created(
            new CriarBeneficioCatalogoCommandResult(benef.Id, benef.Descricao));
    }
}
