using Acme.Sistemas.Core.Helper;
using FluentValidation;

namespace Acme.Sistemas.Services.V1.Cliente.Command.AlterarCliente;

public sealed class AlterarClienteCommandValidation : AbstractValidator<AlterarClienteCommand>
{
    public AlterarClienteCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Documento).NotEmpty()
            .Must(DocumentoHelper.IsValid).WithMessage("CPF/CNPJ inválido.");
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}
