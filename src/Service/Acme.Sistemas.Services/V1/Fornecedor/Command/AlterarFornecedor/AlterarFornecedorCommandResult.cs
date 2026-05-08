using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;

namespace Acme.Sistemas.Services.V1.Fornecedor.Command.AlterarFornecedor;

public sealed record AlterarFornecedorCommandResult(Guid Id);
