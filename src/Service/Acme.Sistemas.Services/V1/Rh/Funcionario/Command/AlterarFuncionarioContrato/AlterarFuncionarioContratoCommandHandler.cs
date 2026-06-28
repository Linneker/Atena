using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AlterarFuncionarioContrato;

public sealed class AlterarFuncionarioContratoCommandHandler
    : IRequestHandler<AlterarFuncionarioContratoCommand, ResponseDefault<AlterarFuncionarioContratoCommandResult>>
{
    private readonly IFuncionarioRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AlterarFuncionarioContratoCommandHandler(IFuncionarioRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarFuncionarioContratoCommandResult>> Handle(
        AlterarFuncionarioContratoCommand request, CancellationToken cancellationToken)
    {
        var func = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (func is null)
            return ResponseDefault<AlterarFuncionarioContratoCommandResult>.NotFound(
                $"Funcionário {request.Id} não encontrado.");

        if (!string.IsNullOrWhiteSpace(request.CodigoMatricula) &&
            !string.Equals(func.CodigoMatricula, request.CodigoMatricula, StringComparison.OrdinalIgnoreCase))
        {
            var conflito = await _repo.GetByMatriculaAsync(request.CodigoMatricula, cancellationToken);
            if (conflito is not null && conflito.Id != request.Id)
                return ResponseDefault<AlterarFuncionarioContratoCommandResult>.Conflict(
                    $"Matrícula '{request.CodigoMatricula}' já está em uso.");
        }

        func.CargoId = request.CargoId;
        func.LotacaoId = request.LotacaoId;
        func.DepartamentoId = request.DepartamentoId;
        func.CentroDeCustoId = request.CentroDeCustoId;
        func.TipoContrato = request.TipoContrato;
        func.RegimeRemuneracao = request.RegimeRemuneracao;
        func.CodigoMatricula = request.CodigoMatricula;
        func.DataDemissao = request.DataDemissao?.ToDateTime(TimeOnly.MinValue);
        func.Status = request.Status;
        func.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(func, cancellationToken);

        return ResponseDefault<AlterarFuncionarioContratoCommandResult>.Ok(
            new AlterarFuncionarioContratoCommandResult(func.Id));
    }
}
