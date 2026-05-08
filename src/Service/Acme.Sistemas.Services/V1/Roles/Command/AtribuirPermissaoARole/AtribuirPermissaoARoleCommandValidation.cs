using FluentValidation;

namespace Acme.Sistemas.Services.V1.Roles.Command.AtribuirPermissaoARole;

public sealed class AtribuirPermissaoARoleCommandValidation : AbstractValidator<AtribuirPermissaoARoleCommand>
{
    public AtribuirPermissaoARoleCommandValidation() { /* sem regras */ }
}
