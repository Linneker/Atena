using Acme.Sistemas.Services.V1.Relatorios.Export;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Acme.Sistemas.Infrastructure.Reports;

public sealed class RelatorioExporter : IRelatorioExporter
{
    public byte[] ToExcel(IReadOnlyList<TabelaExport> tabelas)
    {
        using var wb = new XLWorkbook();
        foreach (var tabela in tabelas)
        {
            var nome = string.IsNullOrWhiteSpace(tabela.Titulo) ? "Relatório" : Truncate(tabela.Titulo, 31);
            var ws = wb.Worksheets.Add(nome);

            for (int c = 0; c < tabela.Colunas.Count; c++)
            {
                ws.Cell(1, c + 1).Value = tabela.Colunas[c];
                ws.Cell(1, c + 1).Style.Font.Bold = true;
                ws.Cell(1, c + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#1F3A93");
                ws.Cell(1, c + 1).Style.Font.FontColor = XLColor.White;
            }

            for (int r = 0; r < tabela.Linhas.Count; r++)
            {
                var linha = tabela.Linhas[r];
                for (int c = 0; c < linha.Count; c++)
                {
                    var cell = ws.Cell(r + 2, c + 1);
                    cell.Value = ToXlValue(linha[c]);
                }
            }

            ws.Columns().AdjustToContents();
        }

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }

    public byte[] ToPdf(string titulo, IReadOnlyList<TabelaExport> tabelas)
    {
        return Document.Create(doc =>
        {
            doc.Page(p =>
            {
                p.Margin(25);
                p.Size(PageSizes.A4.Landscape());
                p.DefaultTextStyle(t => t.FontSize(9));

                p.Header().Column(col =>
                {
                    col.Item().Text(titulo).FontSize(14).Bold().FontColor("#1F3A93");
                    col.Item().PaddingTop(4).LineHorizontal(1).LineColor("#1F3A93");
                });

                p.Content().Column(col =>
                {
                    foreach (var tabela in tabelas)
                    {
                        col.Item().PaddingTop(10).Text(tabela.Titulo).Bold();
                        col.Item().PaddingTop(4).Table(t =>
                        {
                            t.ColumnsDefinition(c =>
                            {
                                foreach (var _ in tabela.Colunas) c.RelativeColumn();
                            });
                            t.Header(h =>
                            {
                                foreach (var coluna in tabela.Colunas)
                                    h.Cell().Background("#1F3A93").Padding(4)
                                        .Text(coluna).FontColor(Colors.White).Bold();
                            });
                            for (int r = 0; r < tabela.Linhas.Count; r++)
                            {
                                var linha = tabela.Linhas[r];
                                foreach (var celula in linha)
                                    t.Cell().Padding(3).Text(FormatCell(celula));
                            }
                        });
                    }
                });

                p.Footer().AlignCenter().Text(t =>
                {
                    t.Span("Atena ERP — gerado em ").FontSize(7).FontColor(Colors.Grey.Medium);
                    t.Span(DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")).FontSize(7).FontColor(Colors.Grey.Medium);
                });
            });
        }).GeneratePdf();
    }

    private static XLCellValue ToXlValue(object? value) => value switch
    {
        null => Blank.Value,
        string s => s,
        DateTime d => d,
        bool b => b,
        decimal m => m,
        double d => d,
        float f => f,
        int i => i,
        long l => l,
        Guid g => g.ToString(),
        _ => value.ToString() ?? string.Empty
    };

    private static string FormatCell(object? value) => value switch
    {
        null => "",
        DateTime d => d.ToString("dd/MM/yyyy"),
        decimal m => m.ToString("N2"),
        double d => d.ToString("N2"),
        _ => value.ToString() ?? ""
    };

    private static string Truncate(string s, int max) => s.Length > max ? s[..max] : s;
}
