using System.Text.Json;
using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Command.AlterarLotacao;

public sealed class AlterarLotacaoCommandValidation : AbstractValidator<AlterarLotacaoCommand>
{
    public AlterarLotacaoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Cnpj).Matches(@"^\d{14}$")
            .When(x => !string.IsNullOrWhiteSpace(x.Cnpj))
            .WithMessage("cnpj deve ter exatamente 14 dígitos.");
        RuleFor(x => x.EnderecoJson).Must(SerJsonValido)
            .When(x => !string.IsNullOrWhiteSpace(x.EnderecoJson))
            .WithMessage("enderecoJson não é um JSON válido.");
    }

    private static bool SerJsonValido(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return true;
        try { using var _ = JsonDocument.Parse(json); return true; }
        catch (JsonException) { return false; }
    }
}
