using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Funcionario.Command.ExcluirFuncionario;

public sealed class ExcluirFuncionarioCommandHandler : IRequestHandler<ExcluirFuncionarioCommand, ResponseDefault>
{
    private readonly IFuncionarioRepository _repo;

    public ExcluirFuncionarioCommandHandler(IFuncionarioRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault> Handle(ExcluirFuncionarioCommand request, CancellationToken cancellationToken)
    {
        var func = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (func is null)
            return ResponseDefault.BadRequest(Error.NotFound("Funcionário não encontrado."));

        await _repo.DeleteAsync(request.Id, cancellationToken);
        return ResponseDefault.NoContent();
    }
}
