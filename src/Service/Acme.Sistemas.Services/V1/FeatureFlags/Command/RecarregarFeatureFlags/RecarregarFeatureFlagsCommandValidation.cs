using FluentValidation;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Command.RecarregarFeatureFlags;

public sealed class RecarregarFeatureFlagsCommandValidation : AbstractValidator<RecarregarFeatureFlagsCommand>
{
    public RecarregarFeatureFlagsCommandValidation() { /* sem regras */ }
}
