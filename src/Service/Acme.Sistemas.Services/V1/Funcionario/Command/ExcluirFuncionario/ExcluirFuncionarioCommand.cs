using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Funcionario.Command.ExcluirFuncionario;

public sealed record ExcluirFuncionarioCommand(Guid Id) : IRequest<ResponseDefault>;
