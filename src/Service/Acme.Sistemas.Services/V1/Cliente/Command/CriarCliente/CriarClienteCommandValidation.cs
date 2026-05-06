using Acme.Sistemas.Core.Helper;
using FluentValidation;

namespace Acme.Sistemas.Services.V1.Cliente.Command.CriarCliente;

public sealed class CriarClienteCommandValidation : AbstractValidator<CriarClienteCommand>
{
    public CriarClienteCommandValidation()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(255);
        RuleFor(x => x.NomeFantasia).MaximumLength(255);
        RuleFor(x => x.Documento).NotEmpty()
            .Must(DocumentoHelper.IsValid).WithMessage("CPF/CNPJ inválido.");
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.Telefone).MaximumLength(30);
    }
}
