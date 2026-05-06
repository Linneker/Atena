using FluentValidation;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.ImportarCertificado;

public sealed class ImportarCertificadoCommandValidation : AbstractValidator<ImportarCertificadoCommand>
{
    public ImportarCertificadoCommandValidation()
    {
        RuleFor(x => x.PfxConteudo).NotNull().Must(b => b.Length > 100)
            .WithMessage("Arquivo PFX vazio ou inválido.");
        RuleFor(x => x.Senha).NotEmpty().MaximumLength(200);
    }
}
