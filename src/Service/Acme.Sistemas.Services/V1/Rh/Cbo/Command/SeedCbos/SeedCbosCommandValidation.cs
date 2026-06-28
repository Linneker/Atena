using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Cbo.Command.SeedCbos;

public sealed class SeedCbosCommandValidation : AbstractValidator<SeedCbosCommand>
{
    public SeedCbosCommandValidation()
    {
        RuleFor(x => x.Cbos).NotNull().Must(c => c.Count > 0)
            .WithMessage("Lista de CBOs não pode estar vazia.");
        RuleForEach(x => x.Cbos).ChildRules(c =>
        {
            c.RuleFor(x => x.Codigo).NotEmpty().Matches(@"^\d{6}$")
                .WithMessage("código CBO deve ter exatamente 6 dígitos numéricos.");
            c.RuleFor(x => x.Titulo).NotEmpty().MaximumLength(255);
            c.RuleFor(x => x.GrandeGrupo).MaximumLength(1);
            c.RuleFor(x => x.Familia).MaximumLength(4);
        });
    }
}
