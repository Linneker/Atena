using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Command.SalvarConfiguracaoRep;

public sealed class SalvarConfiguracaoRepCommandValidation : AbstractValidator<SalvarConfiguracaoRepCommand>
{
    public SalvarConfiguracaoRepCommandValidation()
    {
        RuleFor(x => x.EmpresaId).NotEmpty();
        RuleFor(x => x.RazaoSocial).NotEmpty().MaximumLength(150);
        RuleFor(x => x.CnpjCei).NotEmpty().Length(14);
        RuleFor(x => x.EnderecoLogradouro).NotEmpty().MaximumLength(150);
        RuleFor(x => x.EnderecoCidade).NotEmpty().MaximumLength(80);
        RuleFor(x => x.EnderecoUf).NotEmpty().Length(2);
        RuleFor(x => x.CertificadoId).NotEmpty();
        RuleFor(x => x.ResponsavelCpf).NotEmpty().Length(11);
        RuleFor(x => x.ResponsavelNome).NotEmpty().MaximumLength(150);
    }
}
