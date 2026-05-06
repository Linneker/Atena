using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Command.ExcluirPlanoDeContas;

public sealed class ExcluirPlanoDeContasCommandHandler : IRequestHandler<ExcluirPlanoDeContasCommand, ResponseDefault>
{
    private readonly IPlanoDeContasRepository _repo;

    public ExcluirPlanoDeContasCommandHandler(IPlanoDeContasRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault> Handle(ExcluirPlanoDeContasCommand request, CancellationToken cancellationToken)
    {
        var conta = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (conta is null)
            return ResponseDefault.BadRequest(Error.NotFound("Conta não encontrada."));

        if (await _repo.HasFilhosAsync(conta.Id, cancellationToken))
            return ResponseDefault.BadRequest(Error.Conflict("Não é possível excluir uma conta com filhas. Exclua as filhas primeiro."));

        await _repo.DeleteAsync(conta.Id, cancellationToken);
        return ResponseDefault.NoContent();
    }
}
