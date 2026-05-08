using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Fornecedor.Query.ListarFornecedores;

public sealed record ListarFornecedoresQuery(
    string? Termo = null,
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarFornecedoresQueryResult>>;

