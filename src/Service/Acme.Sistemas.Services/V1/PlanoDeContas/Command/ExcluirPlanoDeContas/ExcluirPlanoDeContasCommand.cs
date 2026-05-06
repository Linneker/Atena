using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Command.ExcluirPlanoDeContas;

public sealed record ExcluirPlanoDeContasCommand(Guid Id) : IRequest<ResponseDefault>;
