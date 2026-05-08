using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;

namespace Acme.Sistemas.Services.V1.Fornecedor.Command.CriarFornecedor;

public sealed record CriarFornecedorCommandResult(Guid Id, string Nome, string Documento);
