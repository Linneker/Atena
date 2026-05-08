using FluentValidation;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Query.ObterFeatureFlag;

public sealed class ObterFeatureFlagQueryValidation : AbstractValidator<ObterFeatureFlagQuery>
{
    public ObterFeatureFlagQueryValidation()
    {
        RuleFor(x => x.Key).NotEmpty().MaximumLength(200);
    }
}
