using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Command.CriarPlanoDeContas;

public sealed record CriarPlanoDeContasCommandResult(Guid Id, string Codigo, string Nome, int Nivel);
