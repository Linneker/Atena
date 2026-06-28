namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auditoria.HistoricoRegistro;

public sealed record HistoricoRegistroRequest(
    string Entidade,
    Guid Id);
