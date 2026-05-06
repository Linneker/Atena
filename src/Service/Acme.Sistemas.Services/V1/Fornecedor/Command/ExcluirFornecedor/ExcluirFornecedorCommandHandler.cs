using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Fornecedor.Command.ExcluirFornecedor;

public sealed class ExcluirFornecedorCommandHandler : IRequestHandler<ExcluirFornecedorCommand, ResponseDefault>
{
    private readonly IFornecedorRepository _repo;

    public ExcluirFornecedorCommandHandler(IFornecedorRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault> Handle(ExcluirFornecedorCommand request, CancellationToken cancellationToken)
    {
        var fornecedor = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (fornecedor is null)
            return ResponseDefault.BadRequest(Error.NotFound("Fornecedor não encontrado."));

        await _repo.DeleteAsync(request.Id, cancellationToken);
        return ResponseDefault.NoContent();
    }
}
