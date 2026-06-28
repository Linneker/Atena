using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Query.ListarDispositivos;

/// <summary>Lista todos os dispositivos do tenant (uso admin).</summary>
public sealed record ListarDispositivosQuery(int Skip = 0, int Take = 50)
    : IRequest<ResponseDefault<ListarDispositivosQueryResult>>;
