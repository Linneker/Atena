using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Command.AlterarPlanoDeContas;

public sealed record AlterarPlanoDeContasCommand(
    Guid Id,
    string Nome,
    bool AceitaLancamento,
    bool Ativo) : IRequest<ResponseDefault<AlterarPlanoDeContasCommandResult>>;

public sealed record AlterarPlanoDeContasCommandResult(Guid Id);
