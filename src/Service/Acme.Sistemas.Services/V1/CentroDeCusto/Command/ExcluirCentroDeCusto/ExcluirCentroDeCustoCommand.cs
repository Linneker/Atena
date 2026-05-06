using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Command.ExcluirCentroDeCusto;

public sealed record ExcluirCentroDeCustoCommand(Guid Id) : IRequest<ResponseDefault>;
