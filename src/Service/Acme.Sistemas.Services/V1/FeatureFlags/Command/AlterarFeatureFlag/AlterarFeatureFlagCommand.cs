using System.Text.Json;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Command.AlterarFeatureFlag;

public sealed record AlterarFeatureFlagCommand(string Key, JsonElement Value)
    : IRequest<ResponseDefault<AlterarFeatureFlagCommandResult>>;
