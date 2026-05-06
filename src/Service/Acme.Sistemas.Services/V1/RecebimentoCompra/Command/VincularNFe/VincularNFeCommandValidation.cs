using Acme.Sistemas.Core.Helper;
using FluentValidation;

namespace Acme.Sistemas.Services.V1.RecebimentoCompra.Command.VincularNFe;

public sealed class VincularNFeCommandValidation : AbstractValidator<VincularNFeCommand>
{
    public VincularNFeCommandValidation()
    {
        RuleFor(x => x.RecebimentoId).NotEmpty();
        RuleFor(x => x.NumeroNotaFiscal).NotEmpty().MaximumLength(30);
        RuleFor(x => x.ChaveAcesso).NotEmpty()
            .Must(NFeChaveAcessoHelper.IsValid)
            .WithMessage("Chave de acesso de NF-e inválida (44 dígitos com DV mód. 11).");
    }
}
