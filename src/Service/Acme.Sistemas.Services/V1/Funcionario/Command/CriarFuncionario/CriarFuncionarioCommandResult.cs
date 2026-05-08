using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Funcionario.Command.CriarFuncionario;

public sealed record CriarFuncionarioCommandResult(Guid Id, string NomeCompleto, string Cpf);
