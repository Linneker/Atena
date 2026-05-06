using FluentValidation;

namespace Acme.Sistemas.Services.V1.Roles.Command.CriarRole;

public sealed class CriarRoleCommandValidation : AbstractValidator<CriarRoleCommand>
{
    public CriarRoleCommandValidation()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Descricao).MaximumLength(500);
    }
}
