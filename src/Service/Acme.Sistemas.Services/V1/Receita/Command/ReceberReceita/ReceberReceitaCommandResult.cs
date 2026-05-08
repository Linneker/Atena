using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Receita.Command.ReceberReceita;

public sealed record ReceberReceitaCommandResult(
    Guid Id,
    StatusPagamento StatusRecebimento,
    decimal ValorRecebido,
    DateTime DataRecebimento);
