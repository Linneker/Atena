using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Divida.Command.ExcluirDivida;

public sealed record ExcluirDividaCommand(Guid Id) : IRequest<ResponseDefault>;
