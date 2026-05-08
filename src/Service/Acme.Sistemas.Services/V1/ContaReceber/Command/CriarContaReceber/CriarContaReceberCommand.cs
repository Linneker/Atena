using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.ContaReceber.Command.CriarContaReceber;

public sealed record CriarContaReceberCommand(
    string Descricao,
    Guid? ClienteId,
    Guid? ReceitaId,
    Guid? PlanoDeContasId,
    decimal ValorOriginal,
    DateTime DataVencimento) : IRequest<ResponseDefault<CriarContaReceberCommandResult>>;

