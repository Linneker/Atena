using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Receita.Command.ExcluirReceita;

public sealed record ExcluirReceitaCommand(Guid Id) : IRequest<ResponseDefault>;
