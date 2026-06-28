using System.Text.Json;
using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Command.AlterarJornada;

public sealed class AlterarJornadaCommandValidation : AbstractValidator<AlterarJornadaCommand>
{
    public AlterarJornadaCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(80);
        RuleFor(x => x.CargaSemanalHoras).GreaterThan(0).LessThanOrEqualTo(60);
        RuleFor(x => x.CargaDiariaHoras).GreaterThan(0).LessThanOrEqualTo(24)
            .When(x => x.CargaDiariaHoras.HasValue);
        RuleFor(x => x.ToleranciaMinutos).InclusiveBetween(0, 60);
        RuleFor(x => x.JanelasJson).NotEmpty()
            .Must(SerJsonValido).WithMessage("janelasJson não é um JSON válido.");
    }

    private static bool SerJsonValido(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return false;
        try { using var _ = JsonDocument.Parse(json); return true; }
        catch (JsonException) { return false; }
    }
}
