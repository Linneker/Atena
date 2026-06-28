using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.CadastrarDependente;

public sealed class CadastrarDependenteCommandHandler
    : IRequestHandler<CadastrarDependenteCommand, ResponseDefault<CadastrarDependenteCommandResult>>
{
    private readonly IDependenteRepository _repo;
    private readonly IFuncionarioRepository _funcRepo;
    private readonly ITenantContext _tenantContext;

    public CadastrarDependenteCommandHandler(
        IDependenteRepository repo,
        IFuncionarioRepository funcRepo,
        ITenantContext tenantContext)
    {
        _repo = repo;
        _funcRepo = funcRepo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CadastrarDependenteCommandResult>> Handle(
        CadastrarDependenteCommand request, CancellationToken cancellationToken)
    {
        var func = await _funcRepo.GetByIdAsync(request.FuncionarioId, cancellationToken);
        if (func is null)
            return ResponseDefault<CadastrarDependenteCommandResult>.NotFound(
                $"Funcionário {request.FuncionarioId} não encontrado.");

        var dep = new Dependente
        {
            TenantId = _tenantContext.TenantId,
            FuncionarioId = request.FuncionarioId,
            NomeCompleto = request.NomeCompleto,
            Cpf = request.Cpf,
            DataNascimento = request.DataNascimento,
            Tipo = request.Tipo,
            Irrf = request.Irrf,
            SalarioFamilia = request.SalarioFamilia,
            PensaoAlimenticiaPct = request.PensaoAlimenticiaPct,
            CreatedBy = _tenantContext.UserId,
        };
        await _repo.AddAsync(dep, cancellationToken);

        return ResponseDefault<CadastrarDependenteCommandResult>.Created(
            new CadastrarDependenteCommandResult(dep.Id));
    }
}
