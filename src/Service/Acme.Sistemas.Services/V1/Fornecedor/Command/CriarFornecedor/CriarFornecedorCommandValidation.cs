using Acme.Sistemas.Core.Helper;
using FluentValidation;

namespace Acme.Sistemas.Services.V1.Fornecedor.Command.CriarFornecedor;

public sealed class CriarFornecedorCommandValidation : AbstractValidator<CriarFornecedorCommand>
{
    public CriarFornecedorCommandValidation()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Documento).NotEmpty()
            .Must(DocumentoHelper.IsValid).WithMessage("CPF/CNPJ inválido.");
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.CondicaoPagamentoPadrao).MaximumLength(100);
    }
}
