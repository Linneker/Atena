using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;

namespace Acme.Sistemas.Services.V1.Empresa.Command.AlterarEmpresa;

public sealed record AlterarEmpresaCommandResult(Guid Id);
