using FluentValidation;

namespace Acme.Sistemas.Services.V1.Admin.Command.SeedTenant;

public sealed class SeedTenantCommandValidation : AbstractValidator<SeedTenantCommand>
{
    public SeedTenantCommandValidation()
    {
        RuleFor(x => x.Cnpj)
            .NotEmpty()
            .Must(SomenteDigitos14)
            .WithMessage("CNPJ deve conter 14 dígitos.");

        RuleFor(x => x.RazaoSocial)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.AdminEmail)
            .NotEmpty()
            .EmailAddress();
    }

    private static bool SomenteDigitos14(string? cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj)) return false;
        var digits = new string(cnpj.Where(char.IsDigit).ToArray());
        return digits.Length == 14;
    }
}
