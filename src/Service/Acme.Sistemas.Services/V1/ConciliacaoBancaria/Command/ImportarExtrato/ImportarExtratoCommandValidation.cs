using FluentValidation;

namespace Acme.Sistemas.Services.V1.ConciliacaoBancaria.Command.ImportarExtrato;

public sealed class ImportarExtratoCommandValidation : AbstractValidator<ImportarExtratoCommand>
{
    public ImportarExtratoCommandValidation()
    {
        RuleFor(x => x.Banco).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Agencia).MaximumLength(20);
        RuleFor(x => x.Conta).MaximumLength(30);
        RuleFor(x => x.Formato).NotEmpty()
            .Must(f => f.Equals("CSV", StringComparison.OrdinalIgnoreCase)
                       || f.Equals("OFX", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Formato deve ser CSV ou OFX.");
        RuleFor(x => x.Conteudo).NotNull().Must(b => b.Length > 0).WithMessage("Conteúdo não pode ser vazio.");
    }
}
