using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Query.ListarPlanoDeContas;

public sealed record ListarPlanoDeContasQuery() : IRequest<ResponseDefault<ListarPlanoDeContasQueryResult>>;

