namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.ConsultarSaldo;

public sealed record ConsultarSaldoRequest(
    Guid ProdutoId,
    Guid? EstoqueId = null);
