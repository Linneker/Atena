using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Usuario.Command.ExcluirUsuario;

public sealed class ExcluirUsuarioCommandHandler : IRequestHandler<ExcluirUsuarioCommand, ResponseDefault>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly ITenantContext _tenantContext;

    public ExcluirUsuarioCommandHandler(IUsuarioRepository usuarios, ITenantContext tenantContext)
    {
        _usuarios = usuarios;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault> Handle(ExcluirUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.GetByIdAsync(request.Id, cancellationToken);
        if (usuario is null)
            return ResponseDefault.BadRequest(Error.NotFound("Usuário não encontrado."));

        if (_tenantContext.UserId == usuario.Id)
            return ResponseDefault.BadRequest(Error.Conflict("Um usuário não pode excluir a si mesmo."));

        var deletedBy = _tenantContext.UserId ?? Guid.Empty;
        await _usuarios.DeleteAsync(request.Id, deletedBy, cancellationToken);

        return ResponseDefault.NoContent();
    }
}
