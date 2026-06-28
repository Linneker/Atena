using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RegistrarReajusteSalarial;

public sealed class RegistrarReajusteSalarialCommandHandler
    : IRequestHandler<RegistrarReajusteSalarialCommand, ResponseDefault<RegistrarReajusteSalarialCommandResult>>
{
    private readonly IFuncionarioRepository _funcRepo;
    private readonly IHistoricoSalarioRepository _histRepo;
    private readonly ITenantContext _tenantContext;

    public RegistrarReajusteSalarialCommandHandler(
        IFuncionarioRepository funcRepo,
        IHistoricoSalarioRepository histRepo,
        ITenantContext tenantContext)
    {
        _funcRepo = funcRepo;
        _histRepo = histRepo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<RegistrarReajusteSalarialCommandResult>> Handle(
        RegistrarReajusteSalarialCommand request, CancellationToken cancellationToken)
    {
        var func = await _funcRepo.GetByIdAsync(request.FuncionarioId, cancellationToken);
        if (func is null)
            return ResponseDefault<RegistrarReajusteSalarialCommandResult>.NotFound(
                $"Funcionário {request.FuncionarioId} não encontrado.");

        var userId = _tenantContext.UserId;

        // Fecha vigência anterior (se houver) no dia anterior à nova vigência.
        Guid? anteriorFechadaId = null;
        var vigenteAnterior = await _histRepo.GetVigenteAsync(
            request.FuncionarioId, request.VigenciaInicio, cancellationToken);

        if (vigenteAnterior is not null && vigenteAnterior.VigenciaFim is null)
        {
            var fimAnterior = request.VigenciaInicio.AddDays(-1);
            // Sanidade: nova vigência não pode ser <= vigência anterior.
            if (fimAnterior < vigenteAnterior.VigenciaInicio)
                return ResponseDefault<RegistrarReajusteSalarialCommandResult>.Conflict(
                    "Nova vigência deve ser posterior à vigência atual em aberto.");

            await _histRepo.FecharVigenciaAsync(vigenteAnterior.Id, fimAnterior, userId, cancellationToken);
            anteriorFechadaId = vigenteAnterior.Id;
        }

        var novo = new HistoricoSalario
        {
            TenantId = _tenantContext.TenantId,
            FuncionarioId = request.FuncionarioId,
            Valor = request.NovoValor,
            VigenciaInicio = request.VigenciaInicio,
            Motivo = request.Motivo,
            Observacao = request.Observacao,
            RegistradoPorUsuarioId = userId,
            RegistradoAt = DateTime.UtcNow,
            CreatedBy = userId,
        };
        await _histRepo.AddAsync(novo, cancellationToken);

        return ResponseDefault<RegistrarReajusteSalarialCommandResult>.Created(
            new RegistrarReajusteSalarialCommandResult(novo.Id, anteriorFechadaId));
    }
}
