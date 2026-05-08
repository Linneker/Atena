using FluentValidation;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Query.ListarFeatureFlags;

public sealed class ListarFeatureFlagsQueryValidation : AbstractValidator<ListarFeatureFlagsQuery>
{
    public ListarFeatureFlagsQueryValidation() { /* sem regras */ }
}
