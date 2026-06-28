using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Command.CriarLotacao;

public sealed record CriarLotacaoCommand(
    string Nome,
    Guid? EmpresaId,
    string? Cnpj,
    string? EnderecoJson) : IRequest<ResponseDefault<CriarLotacaoCommandResult>>;
