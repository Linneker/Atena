using Acme.Sistemas.Services.V1.Relatorios.Financeiro.Balanco;
using Acme.Sistemas.Services.V1.Relatorios.Financeiro.DRE;
using Acme.Sistemas.Services.V1.Relatorios.Pdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Acme.Sistemas.Infrastructure.Reports;

public sealed class QuestPdfRelatorioRenderer : IRelatorioPdfRenderer
{
    static QuestPdfRelatorioRenderer()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] RenderDRE(DREResult dre, TenantBranding branding)
    {
        var corPrimaria = ParseColor(branding.CorPrimariaHex, "#1F3A93");

        return Document.Create(doc =>
        {
            doc.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(t => t.FontSize(10));

                page.Header().Element(c => Header(c, branding, corPrimaria, "Demonstração do Resultado do Exercício"));
                page.Content().Element(c => DREContent(c, dre, corPrimaria));
                page.Footer().AlignCenter().Text(t =>
                {
                    t.Span("Atena ERP — gerado em ").FontSize(8).FontColor(Colors.Grey.Medium);
                    t.Span(DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")).FontSize(8).FontColor(Colors.Grey.Medium);
                });
            });
        }).GeneratePdf();
    }

    public byte[] RenderBalanco(BalancoResult balanco, TenantBranding branding)
    {
        var corPrimaria = ParseColor(branding.CorPrimariaHex, "#1F3A93");

        return Document.Create(doc =>
        {
            doc.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(t => t.FontSize(10));

                page.Header().Element(c => Header(c, branding, corPrimaria, "Balanço Patrimonial Gerencial"));
                page.Content().Element(c => BalancoContent(c, balanco, corPrimaria));
                page.Footer().AlignCenter().Text(t =>
                {
                    t.Span("Atena ERP — gerado em ").FontSize(8).FontColor(Colors.Grey.Medium);
                    t.Span(DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")).FontSize(8).FontColor(Colors.Grey.Medium);
                });
            });
        }).GeneratePdf();
    }

    private static void Header(IContainer container, TenantBranding branding, string corPrimaria, string titulo)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text(branding.RazaoSocial).FontSize(14).Bold().FontColor(corPrimaria);
                    c.Item().Text(titulo).FontSize(12).SemiBold();
                });
            });
            col.Item().PaddingTop(8).LineHorizontal(1).LineColor(corPrimaria);
        });
    }

    private static void DREContent(IContainer container, DREResult dre, string corPrimaria)
    {
        container.PaddingTop(15).Column(col =>
        {
            col.Item().Text($"Período: {dre.Inicio:dd/MM/yyyy} a {dre.Fim:dd/MM/yyyy}").FontSize(10);
            col.Item().PaddingTop(15).Text("RECEITAS").Bold().FontColor(corPrimaria);
            foreach (var linha in dre.Receitas) RenderLinha(col, linha);

            col.Item().PaddingTop(5).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);
            col.Item().Row(r =>
            {
                r.RelativeItem().Text("Total de Receitas").Bold();
                r.ConstantItem(120).AlignRight().Text(dre.TotalReceitas.ToString("C")).Bold();
            });

            col.Item().PaddingTop(15).Text("DESPESAS").Bold().FontColor(corPrimaria);
            foreach (var linha in dre.Despesas) RenderLinha(col, linha);

            col.Item().PaddingTop(5).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);
            col.Item().Row(r =>
            {
                r.RelativeItem().Text("Total de Despesas").Bold();
                r.ConstantItem(120).AlignRight().Text(dre.TotalDespesas.ToString("C")).Bold();
            });

            col.Item().PaddingTop(15).LineHorizontal(1).LineColor(corPrimaria);
            col.Item().PaddingTop(5).Row(r =>
            {
                r.RelativeItem().Text("RESULTADO LÍQUIDO").Bold().FontColor(corPrimaria);
                r.ConstantItem(120).AlignRight().Text(dre.ResultadoLiquido.ToString("C")).Bold().FontColor(corPrimaria);
            });
        });
    }

    private static void RenderLinha(QuestPDF.Fluent.ColumnDescriptor col, DRELinha linha)
    {
        col.Item().PaddingLeft(linha.Nivel * 12).Row(r =>
        {
            r.RelativeItem().Text($"{linha.Codigo} {linha.Nome}");
            r.ConstantItem(120).AlignRight().Text(linha.Total.ToString("C"));
        });
        foreach (var filho in linha.Filhos) RenderLinha(col, filho);
    }

    private static void BalancoContent(IContainer container, BalancoResult b, string corPrimaria)
    {
        container.PaddingTop(15).Column(col =>
        {
            col.Item().Text($"Data de referência: {b.DataReferencia:dd/MM/yyyy}").FontSize(10);

            RenderSecao(col, "ATIVO", b.Ativo, b.TotalAtivo, corPrimaria);
            RenderSecao(col, "PASSIVO", b.Passivo, b.TotalPassivo, corPrimaria);
            RenderSecao(col, "PATRIMÔNIO LÍQUIDO", b.PatrimonioLiquido, b.TotalPatrimonioLiquido, corPrimaria);
        });
    }

    private static void RenderSecao(QuestPDF.Fluent.ColumnDescriptor col, string titulo,
        IReadOnlyList<BalancoLinha> linhas, decimal total, string cor)
    {
        col.Item().PaddingTop(15).Text(titulo).Bold().FontColor(cor);
        foreach (var l in linhas)
        {
            col.Item().Row(r =>
            {
                r.RelativeItem().Text(l.Descricao);
                r.ConstantItem(120).AlignRight().Text(l.Valor.ToString("C"));
            });
        }
        col.Item().PaddingTop(3).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);
        col.Item().Row(r =>
        {
            r.RelativeItem().Text($"Total {titulo}").Bold();
            r.ConstantItem(120).AlignRight().Text(total.ToString("C")).Bold();
        });
    }

    private static string ParseColor(string? hex, string fallback)
    {
        if (string.IsNullOrWhiteSpace(hex)) return fallback;
        var clean = hex.TrimStart('#');
        if (clean.Length is 3 or 6 or 8 && clean.All(c => Uri.IsHexDigit(c))) return "#" + clean;
        return fallback;
    }
}
