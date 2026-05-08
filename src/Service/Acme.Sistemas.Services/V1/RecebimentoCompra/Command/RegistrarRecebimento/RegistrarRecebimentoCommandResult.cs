using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Services.V1.RecebimentoCompra.Command.RegistrarRecebimento;

public sealed record RegistrarRecebimentoCommand(
    Guid PedidoCompraId,
    Guid EstoqueId,
    DateTime? DataRecebimento,
    string? NumeroNotaFiscal,
    string? ChaveAcessoNFe,
    string? Observacao,
    DateTime VencimentoContaPagar,
    Guid? PlanoDeContasId,
    IReadOnlyList<RecebimentoItemDto> Itens) : IRequest<ResponseDefault<RegistrarRecebimentoCommandResult>>;

public sealed record RegistrarRecebimentoCommandResult(
    Guid RecebimentoId,
    TipoRecebimento Tipo,
    Guid? ContaPagarId,
    decimal ValorTotalRecebido,
    int EntradasGeradas);
