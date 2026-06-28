using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using PoliticaEntity = Acme.Sistemas.Domain.Entities.Rh.BancoHorasPolitica;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.CriarPolitica;

public sealed class CriarPoliticaCommandHandler
    : IRequestHandler<CriarPoliticaCommand, ResponseDefault<CriarPoliticaCommandResult>>
{
    private readonly IBancoHorasPoliticaRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarPoliticaCommandHandler(IBancoHorasPoliticaRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarPoliticaCommandResult>> Handle(
        CriarPoliticaCommand request, CancellationToken cancellationToken)
    {
        var existente = await _repo.GetByNomeAsync(request.Nome, cancellationToken);
        if (existente is not null)
            return ResponseDefault<CriarPoliticaCommandResult>.Conflict(
                $"Já existe política com nome '{request.Nome}'.");

        var pol = new PoliticaEntity
        {
            TenantId = _tenantContext.TenantId,
            Nome = request.Nome,
            VigenciaInicio = request.VigenciaInicio,
            VigenciaFim = request.VigenciaFim,
            LimiteHorasAcumular = request.LimiteHorasAcumular,
            PrazoCompensacaoDias = request.PrazoCompensacaoDias,
            PermitePagarExcedente = request.PermitePagarExcedente,
            FatorPagamento = request.FatorPagamento,
            Ativo = true,
            CreatedBy = _tenantContext.UserId,
        };
        await _repo.AddAsync(pol, cancellationToken);
        return ResponseDefault<CriarPoliticaCommandResult>.Created(
            new CriarPoliticaCommandResult(pol.Id, pol.Nome));
    }
}
