using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.EnviarParaAprovacao;

public sealed record EnviarParaAprovacaoCommand(Guid Id)
    : IRequest<ResponseDefault<EnviarParaAprovacaoCommandResult>>;

public sealed record EnviarParaAprovacaoCommandResult(Guid Id, decimal ValorTotal, string PermissaoNecessaria);
