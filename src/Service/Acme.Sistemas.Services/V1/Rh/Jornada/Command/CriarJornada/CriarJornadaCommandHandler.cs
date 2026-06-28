using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using JornadaEntity = Acme.Sistemas.Domain.Entities.Rh.Jornada;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Command.CriarJornada;

public sealed class CriarJornadaCommandHandler
    : IRequestHandler<CriarJornadaCommand, ResponseDefault<CriarJornadaCommandResult>>
{
    private readonly IJornadaRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarJornadaCommandHandler(IJornadaRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarJornadaCommandResult>> Handle(
        CriarJornadaCommand request, CancellationToken cancellationToken)
    {
        var existente = await _repo.GetByNomeAsync(request.Nome, cancellationToken);
        if (existente is not null)
            return ResponseDefault<CriarJornadaCommandResult>.Conflict(
                $"Já existe uma jornada com o nome '{request.Nome}'.");

        var jornada = new JornadaEntity
        {
            TenantId = _tenantContext.TenantId,
            Nome = request.Nome,
            Tipo = request.Tipo,
            CargaSemanalHoras = request.CargaSemanalHoras,
            CargaDiariaHoras = request.CargaDiariaHoras,
            JanelasJson = request.JanelasJson,
            PermiteMarcarIntervalo = request.PermiteMarcarIntervalo,
            ToleranciaMinutos = request.ToleranciaMinutos,
            Ativo = true,
            CreatedBy = _tenantContext.UserId,
        };

        await _repo.AddAsync(jornada, cancellationToken);

        return ResponseDefault<CriarJornadaCommandResult>.Created(
            new CriarJornadaCommandResult(jornada.Id, jornada.Nome));
    }
}
