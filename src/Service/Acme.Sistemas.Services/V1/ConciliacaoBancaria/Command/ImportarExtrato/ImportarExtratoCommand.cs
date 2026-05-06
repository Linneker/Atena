using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.ConciliacaoBancaria.Command.ImportarExtrato;

public sealed record ImportarExtratoCommand(
    string Banco,
    string? Agencia,
    string? Conta,
    string Formato,
    byte[] Conteudo) : IRequest<ResponseDefault<ImportarExtratoCommandResult>>;

public sealed record ImportarExtratoCommandResult(
    Guid ConciliacaoId,
    int TotalLancamentos,
    int TotalConciliados);
