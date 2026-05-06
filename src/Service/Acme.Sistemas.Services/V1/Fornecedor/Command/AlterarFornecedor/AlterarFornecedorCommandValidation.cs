using Acme.Sistemas.Core.Helper;
using FluentValidation;

namespace Acme.Sistemas.Services.V1.Fornecedor.Command.AlterarFornecedor;

public sealed class AlterarFornecedorCommandValidation : AbstractValidator<AlterarFornecedorCommand>
{
    public AlterarFornecedorCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Documento).NotEmpty()
            .Must(DocumentoHelper.IsValid).WithMessage("CPF/CNPJ inválido.");
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}
