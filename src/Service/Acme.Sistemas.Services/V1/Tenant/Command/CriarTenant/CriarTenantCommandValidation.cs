using FluentValidation;

namespace Acme.Sistemas.Services.V1.Tenant.Command.CriarTenant;

public sealed class CriarTenantCommandValidation : AbstractValidator<CriarTenantCommand>
{
    public CriarTenantCommandValidation()
    {
        RuleFor(x => x.RazaoSocial).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Cnpj).NotEmpty().Length(14, 18).Must(BeValidCnpj).WithMessage("CNPJ inválido.");
        RuleFor(x => x.Plano).NotEmpty().Must(p => p is "FREE" or "BASIC" or "PRO" or "ENTERPRISE")
            .WithMessage("Plano inválido.");
        RuleFor(x => x.CorPrimaria).MaximumLength(20);
        RuleFor(x => x.LogoUrl).MaximumLength(500);
        RuleFor(x => x.AdminNomeCompleto).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdminEmail).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.AdminSenha).NotEmpty().MinimumLength(8).MaximumLength(100);
    }

    private static bool BeValidCnpj(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj)) return false;
        var digits = new string(cnpj.Where(char.IsDigit).ToArray());
        if (digits.Length != 14) return false;
        if (digits.Distinct().Count() == 1) return false;

        var multipliers1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multipliers2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var sum1 = 0;
        for (var i = 0; i < 12; i++) sum1 += (digits[i] - '0') * multipliers1[i];
        var rem = sum1 % 11;
        var d1 = rem < 2 ? 0 : 11 - rem;
        if (digits[12] - '0' != d1) return false;

        var sum2 = 0;
        for (var i = 0; i < 13; i++) sum2 += (digits[i] - '0') * multipliers2[i];
        rem = sum2 % 11;
        var d2 = rem < 2 ? 0 : 11 - rem;
        return digits[13] - '0' == d2;
    }
}
