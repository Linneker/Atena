using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.AprovarSolicitacao;

public sealed record AprovarSolicitacaoCommand(Guid Id) : IRequest<ResponseDefault<AprovarSolicitacaoCommandResult>>;

public sealed record AprovarSolicitacaoCommandResult(Guid Id, DateTime AprovadoEm);
