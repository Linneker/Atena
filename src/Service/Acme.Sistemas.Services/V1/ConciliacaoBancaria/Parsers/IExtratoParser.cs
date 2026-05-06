using Acme.Sistemas.Domain.Entities.Financeiro;

namespace Acme.Sistemas.Services.V1.ConciliacaoBancaria.Parsers;

public interface IExtratoParser
{
    string Formato { get; }
    IReadOnlyList<ParsedExtratoItem> Parse(Stream content);
}

public sealed record ParsedExtratoItem(
    DateTime DataMovimento,
    decimal Valor,
    TipoMovimentoExtrato Tipo,
    string? Descricao,
    string? DocumentoBancario);
