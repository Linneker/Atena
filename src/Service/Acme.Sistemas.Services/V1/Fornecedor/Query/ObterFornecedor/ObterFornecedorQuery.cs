using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Fornecedor.Query.ObterFornecedor;

public sealed record ObterFornecedorQuery(Guid Id) : IRequest<ResponseDefault<ObterFornecedorQueryResult>>;

