using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.VincularBeneficio;

public sealed class VincularBeneficioCommandHandler
    : IRequestHandler<VincularBeneficioCommand, ResponseDefault<VincularBeneficioCommandResult>>
{
    private readonly IBeneficioFuncionarioRepository _repo;
    private readonly IFuncionarioRepository _funcRepo;
    private readonly ITenantContext _tenantContext;

    public VincularBeneficioCommandHandler(
        IBeneficioFuncionarioRepository repo,
        IFuncionarioRepository funcRepo,
        ITenantContext tenantContext)
    {
        _repo = repo;
        _funcRepo = funcRepo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<VincularBeneficioCommandResult>> Handle(
        VincularBeneficioCommand request, CancellationToken cancellationToken)
    {
        var func = await _funcRepo.GetByIdAsync(request.FuncionarioId, cancellationToken);
        if (func is null)
            return ResponseDefault<VincularBeneficioCommandResult>.NotFound(
                $"Funcionário {request.FuncionarioId} não encontrado.");

        var vigente = await _repo.GetVigenteAsync(
            request.FuncionarioId, request.BeneficioCatalogoId, request.VigenciaInicio, cancellationToken);
        if (vigente is not null)
            return ResponseDefault<VincularBeneficioCommandResult>.Conflict(
                "Funcionário já possui esse benefício vigente na data informada.");

        var vinculo = new BeneficioFuncionario
        {
            TenantId = _tenantContext.TenantId,
            FuncionarioId = request.FuncionarioId,
            BeneficioCatalogoId = request.BeneficioCatalogoId,
            Valor = request.Valor,
            DescontoFuncionarioPct = request.DescontoFuncionarioPct,
            VigenciaInicio = request.VigenciaInicio,
            Observacao = request.Observacao,
            CreatedBy = _tenantContext.UserId,
        };
        await _repo.AddAsync(vinculo, cancellationToken);

        return ResponseDefault<VincularBeneficioCommandResult>.Created(
            new VincularBeneficioCommandResult(vinculo.Id));
    }
}
