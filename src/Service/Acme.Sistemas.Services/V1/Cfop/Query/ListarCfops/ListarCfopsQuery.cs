using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Cfop.Query.ListarCfops;

public sealed record ListarCfopsQuery(string? Categoria = null)
    : IRequest<ResponseDefault<ListarCfopsQueryResult>>;
