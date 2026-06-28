namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.EnviarParaAprovacao;

public sealed record EnviarParaAprovacaoResponse(
    Guid Id,
    decimal ValorTotal,
    string PermissaoNecessaria);
