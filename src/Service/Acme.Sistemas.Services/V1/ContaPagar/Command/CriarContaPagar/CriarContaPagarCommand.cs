using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.ContaPagar.Command.CriarContaPagar;

public sealed record CriarContaPagarCommand(
    string Descricao,
    Guid? FornecedorId,
    Guid? DespesaId,
    Guid? PlanoDeContasId,
    decimal ValorOriginal,
    DateTime DataVencimento,
    string? Observacao) : IRequest<ResponseDefault<CriarContaPagarCommandResult>>;

public sealed record CriarContaPagarCommandResult(Guid Id, string Descricao, decimal ValorOriginal, DateTime DataVencimento);
