using FluentValidation;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Command.AlterarFeatureFlag;

public sealed class AlterarFeatureFlagCommandValidation : AbstractValidator<AlterarFeatureFlagCommand>
{
    public AlterarFeatureFlagCommandValidation()
    {
        RuleFor(x => x.Key).NotEmpty().MaximumLength(200);
    }
}
