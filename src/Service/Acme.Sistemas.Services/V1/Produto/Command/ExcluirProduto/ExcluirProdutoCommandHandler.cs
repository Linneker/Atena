using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Produto.Command.ExcluirProduto;

public sealed class ExcluirProdutoCommandHandler : IRequestHandler<ExcluirProdutoCommand, ResponseDefault>
{
    private readonly IProdutoRepository _repo;

    public ExcluirProdutoCommandHandler(IProdutoRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault> Handle(ExcluirProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (produto is null)
            return ResponseDefault.BadRequest(Error.NotFound("Produto não encontrado."));

        await _repo.DeleteAsync(request.Id, cancellationToken);
        return ResponseDefault.NoContent();
    }
}
