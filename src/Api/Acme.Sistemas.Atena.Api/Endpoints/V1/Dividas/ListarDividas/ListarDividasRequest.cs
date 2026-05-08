using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dividas.ListarDividas;

public sealed record ListarDividasRequest(StatusConta? Status = null, int? Skip = null, int? Take = null);
