using Acme.Sistemas.Core.Helper;
using FluentValidation;

namespace Acme.Sistemas.Services.V1.Empresa.Command.AlterarEmpresa;

public sealed class AlterarEmpresaCommandValidation : AbstractValidator<AlterarEmpresaCommand>
{
    public AlterarEmpresaCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RazaoSocial).NotEmpty().MaximumLength(255);
        RuleFor(x => x.NomeFantasia).MaximumLength(255);
        RuleFor(x => x.Cnpj).NotEmpty()
            .Must(CnpjHelper.IsValid).WithMessage("CNPJ inválido.");
        RuleFor(x => x.InscricaoEstadual).MaximumLength(50);
        RuleFor(x => x.InscricaoMunicipal).MaximumLength(50);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.Telefone).MaximumLength(30);
        RuleFor(x => x.Status).IsInEnum();
    }
}
