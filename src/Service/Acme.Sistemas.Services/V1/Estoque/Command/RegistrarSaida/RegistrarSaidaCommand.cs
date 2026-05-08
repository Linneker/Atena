using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Estoque;

namespace Acme.Sistemas.Services.V1.Estoque.Command.RegistrarSaida;

public sealed record RegistrarSaidaCommand(
    Guid EstoqueId,
    Guid ProdutoId,
    decimal Quantidade,
    decimal? CustoUnitario,
    OrigemMovimento Origem,
    string? Motivo,
    Guid? ClienteId,
    string? DocumentoReferencia,
    DateTime? DataMovimento) : IRequest<ResponseDefault<RegistrarSaidaCommandResult>>;

