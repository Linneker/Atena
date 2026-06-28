using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.RecebimentoCompra.RegistrarRecebimento;

public sealed record RegistrarRecebimentoResponse(
    Guid RecebimentoId,
    TipoRecebimento Tipo,
    Guid? ContaPagarId,
    decimal ValorTotalRecebido,
    int EntradasGeradas);
