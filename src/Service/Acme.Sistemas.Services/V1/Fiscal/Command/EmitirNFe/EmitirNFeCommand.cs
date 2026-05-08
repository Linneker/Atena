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

