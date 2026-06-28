using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AtribuirEscala;

public sealed class AtribuirEscalaCommandHandler
    : IRequestHandler<AtribuirEscalaCommand, ResponseDefault<AtribuirEscalaCommandResult>>
{
    private readonly IEscalaFuncionarioRepository _repo;
    private readonly IFuncionarioRepository _funcRepo;
    private readonly ITenantContext _tenantContext;

    public AtribuirEscalaCommandHandler(
        IEscalaFuncionarioRepository repo,
        IFuncionarioRepository funcRepo,
        ITenantContext tenantContext)
    {
        _repo = repo;
        _funcRepo = funcRepo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AtribuirEscalaCommandResult>> Handle(
        AtribuirEscalaCommand request, CancellationToken cancellationToken)
    {
        var func = await _funcRepo.GetByIdAsync(request.FuncionarioId, cancellationToken);
        if (func is null)
            return ResponseDefault<AtribuirEscalaCommandResult>.NotFound(
                $"Funcionário {request.FuncionarioId} não encontrado.");

        var userId = _tenantContext.UserId;

        Guid? anteriorFechadaId = null;
        var vigenteAnterior = await _repo.GetVigenteAsync(
            request.FuncionarioId, request.VigenciaInicio, cancellationToken);

        if (vigenteAnterior is not null && vigenteAnterior.VigenciaFim is null)
        {
            var fimAnterior = request.VigenciaInicio.AddDays(-1);
            if (fimAnterior < vigenteAnterior.VigenciaInicio)
                return ResponseDefault<AtribuirEscalaCommandResult>.Conflict(
                    "Nova vigência deve ser posterior à escala em vigor.");

            await _repo.FecharVigenciaAsync(vigenteAnterior.Id, fimAnterior, userId, cancellationToken);
            anteriorFechadaId = vigenteAnterior.Id;
        }

        var nova = new EscalaFuncionario
        {
            TenantId = _tenantContext.TenantId,
            FuncionarioId = request.FuncionarioId,
            JornadaId = request.JornadaId,
            VigenciaInicio = request.VigenciaInicio,
            Observacao = request.Observacao,
            CreatedBy = userId,
        };
        await _repo.AddAsync(nova, cancellationToken);

        return ResponseDefault<AtribuirEscalaCommandResult>.Created(
            new AtribuirEscalaCommandResult(nova.Id, anteriorFechadaId));
    }
}
