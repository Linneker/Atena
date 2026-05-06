using Acme.Sistemas.Core.Helper;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using FuncionarioEntity = Acme.Sistemas.Domain.Entities.Cadastros.Funcionario;

namespace Acme.Sistemas.Services.V1.Funcionario.Command.CriarFuncionario;

public sealed class CriarFuncionarioCommandHandler
    : IRequestHandler<CriarFuncionarioCommand, ResponseDefault<CriarFuncionarioCommandResult>>
{
    private readonly IFuncionarioRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarFuncionarioCommandHandler(IFuncionarioRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarFuncionarioCommandResult>> Handle(CriarFuncionarioCommand request, CancellationToken cancellationToken)
    {
        var cpf = CpfHelper.OnlyDigits(request.Cpf);
        var existing = await _repo.GetByCpfAsync(cpf, cancellationToken);
        if (existing is not null)
            return ResponseDefault<CriarFuncionarioCommandResult>.Conflict(
                $"Já existe um funcionário com o CPF {cpf}.");

        var func = new FuncionarioEntity
        {
            TenantId = _tenantContext.TenantId,
            NomeCompleto = request.NomeCompleto,
            Cpf = cpf,
            Email = request.Email,
            Telefone = request.Telefone,
            Cargo = request.Cargo,
            Departamento = request.Departamento,
            CentroDeCustoId = request.CentroDeCustoId,
            DataAdmissao = request.DataAdmissao,
            UsuarioId = request.UsuarioId,
            Status = StatusAtivo.Ativo,
            CreatedBy = _tenantContext.UserId
        };

        await _repo.AddAsync(func, cancellationToken);
        return ResponseDefault<CriarFuncionarioCommandResult>.Created(
            new CriarFuncionarioCommandResult(func.Id, func.NomeCompleto, func.Cpf));
    }
}
