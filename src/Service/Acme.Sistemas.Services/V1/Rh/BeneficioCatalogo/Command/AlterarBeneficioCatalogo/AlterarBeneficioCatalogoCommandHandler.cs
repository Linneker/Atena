using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Command.AlterarBeneficioCatalogo;

public sealed class AlterarBeneficioCatalogoCommandHandler
    : IRequestHandler<AlterarBeneficioCatalogoCommand, ResponseDefault<AlterarBeneficioCatalogoCommandResult>>
{
    private readonly IBeneficioCatalogoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AlterarBeneficioCatalogoCommandHandler(IBeneficioCatalogoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarBeneficioCatalogoCommandResult>> Handle(
        AlterarBeneficioCatalogoCommand request, CancellationToken cancellationToken)
    {
        var benef = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (benef is null)
            return ResponseDefault<AlterarBeneficioCatalogoCommandResult>.NotFound(
                $"Benefício {request.Id} não encontrado.");

        if (!string.IsNullOrWhiteSpace(request.Codigo) &&
            !string.Equals(benef.Codigo, request.Codigo, StringComparison.OrdinalIgnoreCase))
        {
            var conflito = await _repo.GetByCodigoAsync(request.Codigo, cancellationToken);
            if (conflito is not null && conflito.Id != request.Id)
                return ResponseDefault<AlterarBeneficioCatalogoCommandResult>.Conflict(
                    $"Já existe um benefício com o código '{request.Codigo}'.");
        }

        benef.Codigo = request.Codigo;
        benef.Descricao = request.Descricao;
        benef.Tipo = request.Tipo;
        benef.DescontoFuncionarioPct = request.DescontoFuncionarioPct;
        benef.CustoEmpresaPadrao = request.CustoEmpresaPadrao;
        benef.NaturezaRubricaEsocial = request.NaturezaRubricaEsocial;
        benef.Ativo = request.Ativo;
        benef.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(benef, cancellationToken);

        return ResponseDefault<AlterarBeneficioCatalogoCommandResult>.Ok(
            new AlterarBeneficioCatalogoCommandResult(benef.Id));
    }
}
