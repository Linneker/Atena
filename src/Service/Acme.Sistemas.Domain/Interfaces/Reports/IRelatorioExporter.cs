namespace Acme.Sistemas.Domain.Interfaces.Reports;

public sealed record TabelaExport(string Titulo, IReadOnlyList<string> Colunas, IReadOnlyList<IReadOnlyList<object?>> Linhas);

public interface IRelatorioExporter
{
    byte[] ToExcel(IReadOnlyList<TabelaExport> tabelas);
    byte[] ToPdf(string titulo, IReadOnlyList<TabelaExport> tabelas);
}
