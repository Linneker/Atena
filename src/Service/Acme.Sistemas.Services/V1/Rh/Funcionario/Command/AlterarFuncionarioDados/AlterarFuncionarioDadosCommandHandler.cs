using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AlterarFuncionarioDados;

public sealed class AlterarFuncionarioDadosCommandHandler
    : IRequestHandler<AlterarFuncionarioDadosCommand, ResponseDefault<AlterarFuncionarioDadosCommandResult>>
{
    private readonly IFuncionarioRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AlterarFuncionarioDadosCommandHandler(IFuncionarioRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarFuncionarioDadosCommandResult>> Handle(
        AlterarFuncionarioDadosCommand request, CancellationToken cancellationToken)
    {
        var func = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (func is null)
            return ResponseDefault<AlterarFuncionarioDadosCommandResult>.NotFound(
                $"Funcionário {request.Id} não encontrado.");

        func.NomeCompleto = request.NomeCompleto;
        func.Email = request.Email;
        func.Telefone = request.Telefone;
        func.Rg = request.Rg;
        func.RgOrgao = request.RgOrgao;
        func.RgUf = request.RgUf;
        func.EstadoCivil = request.EstadoCivil;
        func.Naturalidade = request.Naturalidade;
        func.Nacionalidade = request.Nacionalidade ?? func.Nacionalidade;
        func.Endereco = request.Endereco ?? func.Endereco;
        func.ContaBancaria = request.ContaBancaria ?? func.ContaBancaria;
        func.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(func, cancellationToken);

        return ResponseDefault<AlterarFuncionarioDadosCommandResult>.Ok(
            new AlterarFuncionarioDadosCommandResult(func.Id));
    }
}
