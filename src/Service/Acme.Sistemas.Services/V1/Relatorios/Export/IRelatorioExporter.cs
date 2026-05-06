namespace Acme.Sistemas.Services.V1.Relatorios.Export;

public sealed record TabelaExport(string Titulo, IReadOnlyList<string> Colunas, IReadOnlyList<IReadOnlyList<object?>> Linhas);

public interface IRelatorioExporter
{
    /// <summary>Exporta tabela(s) para .xlsx (uma planilha por tabela).</summary>
    byte[] ToExcel(IReadOnlyList<TabelaExport> tabelas);

    /// <summary>Exporta tabela(s) para PDF tabular básico.</summary>
    byte[] ToPdf(string titulo, IReadOnlyList<TabelaExport> tabelas);
}
