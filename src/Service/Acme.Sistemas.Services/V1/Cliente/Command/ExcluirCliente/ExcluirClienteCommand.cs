using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Cliente.Command.ExcluirCliente;

public sealed record ExcluirClienteCommand(Guid Id) : IRequest<ResponseDefault>;
