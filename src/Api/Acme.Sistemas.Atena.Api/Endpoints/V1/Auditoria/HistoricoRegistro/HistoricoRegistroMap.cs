using Acme.Sistemas.Services.V1.Auditoria.Query.HistoricoRegistro;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auditoria.HistoricoRegistro;

public static class HistoricoRegistroMap
{
    public static HistoricoRegistroQuery ToQuery(this HistoricoRegistroRequest request)
        => new(request.Entidade, request.Id);

    public static HistoricoRegistroResponse ToResponse(this HistoricoRegistroQueryResult result)
        => new(
            result.Entidade,
            result.EntidadeId,
            result.Eventos.Select(e => e.ToResponseEvento()).ToArray());

    private static HistoricoRegistroResponseEvento ToResponseEvento(this HistoricoRegistroItem item)
        => new(item.Id, item.UserId, item.Operacao, item.CommandTipo, item.AntesJson, item.DepoisJson, item.OcorridoEm);
}
