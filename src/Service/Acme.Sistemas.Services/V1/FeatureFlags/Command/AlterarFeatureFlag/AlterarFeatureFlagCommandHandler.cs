using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Domain.Interfaces.AppConfiguration;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Command.AlterarFeatureFlag;

public sealed class AlterarFeatureFlagCommandHandler
    : IRequestHandler<AlterarFeatureFlagCommand, ResponseDefault<AlterarFeatureFlagCommandResult>>
{
    private readonly IFeatureFlagService _service;

    public AlterarFeatureFlagCommandHandler(IFeatureFlagService service) => _service = service;

    public async Task<ResponseDefault<AlterarFeatureFlagCommandResult>> Handle(
        AlterarFeatureFlagCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _service.SetAsync(request.Key, request.Value, cancellationToken);
        }
        catch (ArgumentException ex)
        {
            return ResponseDefault<AlterarFeatureFlagCommandResult>.BadRequest(
                Error.Validation(ex.Message));
        }

        var updated = _service.Get(request.Key);
        return updated is null
            ? ResponseDefault<AlterarFeatureFlagCommandResult>.NotFound($"Flag '{request.Key}' não encontrada.")
            : ResponseDefault<AlterarFeatureFlagCommandResult>.Ok(
                new AlterarFeatureFlagCommandResult(updated.Key, updated.Value, updated.Type.ToString()));
    }
}
