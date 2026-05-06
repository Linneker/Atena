using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.EmitirNFe;

public sealed record NFeItemDto(
    Guid ProdutoId,
    string Descricao,
    decimal Quantidade,
    decimal PrecoUnitario,
    string? Cfop,
    string? Ncm);

public sealed record EmitirNFeCommand(
    Guid? FaturamentoId,
    Guid ClienteId,
    IReadOnlyList<NFeItemDto> Itens) : IRequest<ResponseDefault<EmitirNFeCommandResult>>;

public sealed record EmitirNFeCommandResult(
    Guid NFeId,
    int Numero,
    int Serie,
    string ChaveAcesso,
    bool EnfileiradaParaTransmissao);
