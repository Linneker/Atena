using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Services.V1.Rh.Ponto.Engine;
using MarcacaoEntity = Acme.Sistemas.Domain.Entities.Rh.MarcacaoPonto;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.IncluirMarcacaoManual;

public sealed class IncluirMarcacaoManualCommandHandler
    : IRequestHandler<IncluirMarcacaoManualCommand, ResponseDefault<IncluirMarcacaoManualCommandResult>>
{
    private readonly IMarcacaoPontoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public IncluirMarcacaoManualCommandHandler(IMarcacaoPontoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<IncluirMarcacaoManualCommandResult>> Handle(
        IncluirMarcacaoManualCommand request, CancellationToken cancellationToken)
    {
        var ultima = await _repo.GetUltimaPorFuncionarioAsync(request.FuncionarioId, cancellationToken);
        var origem = OrigemMarcacao.Manual;
        var hash = MarcacaoPontoIntegridade.Calcular(
            request.FuncionarioId, request.DataHora, request.Tipo, origem, ultima?.HashIntegridade);

        var marcacao = new MarcacaoEntity
        {
            TenantId = _tenantContext.TenantId,
            FuncionarioId = request.FuncionarioId,
            Tipo = request.Tipo,
            DataHora = request.DataHora,
            Origem = origem,
            HashAnterior = ultima?.HashIntegridade,
            HashIntegridade = hash,
            Status = StatusMarcacao.Valida,
            CreatedBy = _tenantContext.UserId,
        };

        await _repo.AddAsync(marcacao, cancellationToken);
        return ResponseDefault<IncluirMarcacaoManualCommandResult>.Created(
            new IncluirMarcacaoManualCommandResult(marcacao.Id, marcacao.HashIntegridade));
    }
}
