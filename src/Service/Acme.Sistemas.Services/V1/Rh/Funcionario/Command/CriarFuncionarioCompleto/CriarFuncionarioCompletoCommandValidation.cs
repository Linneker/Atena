using Acme.Sistemas.Core.Helper;
using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.CriarFuncionarioCompleto;

public sealed class CriarFuncionarioCompletoCommandValidation : AbstractValidator<CriarFuncionarioCompletoCommand>
{
    public CriarFuncionarioCompletoCommandValidation()
    {
        RuleFor(x => x.NomeCompleto).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Cpf).NotEmpty()
            .Must(CpfHelper.IsValid).WithMessage("cpf é inválido (DV não bate).");
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.Pis).Must(p => PisHelper.IsValid(p!))
            .When(x => !string.IsNullOrWhiteSpace(x.Pis))
            .WithMessage("pis é inválido (DV não bate).");
        RuleFor(x => x).Must(x => CtpsHelper.IsValid(x.Ctps, x.CtpsSerie, x.CtpsUf))
            .When(x => !string.IsNullOrWhiteSpace(x.Ctps)
                       || !string.IsNullOrWhiteSpace(x.CtpsSerie)
                       || !string.IsNullOrWhiteSpace(x.CtpsUf))
            .WithMessage("ctps inválida — informe número, série e UF válidos quando preencher CTPS.");
        RuleFor(x => x.SalarioInicial).GreaterThan(0)
            .WithMessage("salário inicial deve ser maior que zero.");
        RuleFor(x => x.CodigoMatricula).MaximumLength(30);
        RuleFor(x => x.RgUf).Length(2).When(x => !string.IsNullOrWhiteSpace(x.RgUf));
        RuleFor(x => x.ContaBancaria!).Must(c => ContaBancariaHelper.IsValid(
                c.CodigoBanco, c.Agencia, c.AgenciaDigito, c.Conta, c.ContaDigito))
            .When(x => x.ContaBancaria is not null &&
                       (!string.IsNullOrWhiteSpace(x.ContaBancaria.CodigoBanco)
                        || !string.IsNullOrWhiteSpace(x.ContaBancaria.Agencia)
                        || !string.IsNullOrWhiteSpace(x.ContaBancaria.Conta)))
            .WithMessage("contaBancaria inválida — banco/agência/conta com formato incorreto.");
    }
}
