using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Cliente.Command.ExcluirCliente;

public sealed class ExcluirClienteCommandHandler : IRequestHandler<ExcluirClienteCommand, ResponseDefault>
{
    private readonly IClienteRepository _repo;

    public ExcluirClienteCommandHandler(IClienteRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault> Handle(ExcluirClienteCommand request, CancellationToken cancellationToken)
    {
        var cliente = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (cliente is null)
            return ResponseDefault.BadRequest(Error.NotFound("Cliente não encontrado."));

        await _repo.DeleteAsync(request.Id, cancellationToken);
        return ResponseDefault.NoContent();
    }
}
