using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Funcionario.Command.AlterarFuncionario;

public sealed class AlterarFuncionarioCommandHandler
    : IRequestHandler<AlterarFuncionarioCommand, ResponseDefault<AlterarFuncionarioCommandResult>>
{
    private readonly IFuncionarioRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AlterarFuncionarioCommandHandler(IFuncionarioRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarFuncionarioCommandResult>> Handle(AlterarFuncionarioCommand request, CancellationToken cancellationToken)
    {
        var func = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (func is null)
            return ResponseDefault<AlterarFuncionarioCommandResult>.NotFound("Funcionário não encontrado.");

        func.NomeCompleto = request.NomeCompleto;
        func.Email = request.Email;
        func.Telefone = request.Telefone;
        func.Cargo = request.Cargo;
        func.Departamento = request.Departamento;
        func.CentroDeCustoId = request.CentroDeCustoId;
        func.DataAdmissao = request.DataAdmissao;
        func.DataDemissao = request.DataDemissao;
        func.UsuarioId = request.UsuarioId;
        func.Status = request.Status;
        func.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(func, cancellationToken);
        return ResponseDefault<AlterarFuncionarioCommandResult>.Ok(new AlterarFuncionarioCommandResult(func.Id));
    }
}
