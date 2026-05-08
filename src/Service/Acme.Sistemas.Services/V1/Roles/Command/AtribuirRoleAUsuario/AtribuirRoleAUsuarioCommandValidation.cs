using FluentValidation;

namespace Acme.Sistemas.Services.V1.Roles.Command.AtribuirRoleAUsuario;

public sealed class AtribuirRoleAUsuarioCommandValidation : AbstractValidator<AtribuirRoleAUsuarioCommand>
{
    public AtribuirRoleAUsuarioCommandValidation() { /* sem regras */ }
}
