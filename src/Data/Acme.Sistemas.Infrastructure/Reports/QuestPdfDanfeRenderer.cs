using Acme.Sistemas.Services.V1.Relatorios.Pdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Acme.Sistemas.Infrastructure.Reports;

/// <summary>
/// DANFE simplificado — layout reduzido para fluxo de teste.
/// O DANFE oficial segue layout MOC-NFE-DANFE; aqui geramos uma versão informativa.
/// </summary>
public sealed class QuestPdfDanfeRenderer : IDanfePdfRenderer
{
    public byte[] Render(DanfeData data, TenantBranding branding)
    {
        var cor = ParseColor(branding.CorPrimariaHex, "#1F3A93");

        return Document.Create(doc =>
        {
            doc.Page(p =>
            {
                p.Margin(25);
                p.Size(PageSizes.A4);
                p.DefaultTextStyle(t => t.FontSize(9));

                p.Header().Element(c => Header(c, data, cor));
                p.Content().Element(c => Body(c, data, cor));
                p.Footer().AlignCenter().Text(t =>
                {
                    t.Span("DANFE simplificado — gerado em ").FontSize(7).FontColor(Colors.Grey.Medium);
                    t.Span(DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")).FontSize(7).FontColor(Colors.Grey.Medium);
                });
            });
        }).GeneratePdf();
    }

    private static void Header(IContainer c, DanfeData d, string cor)
    {
        c.Column(col =>
        {
            col.Item().Row(r =>
            {
                r.RelativeItem().Column(rc =>
                {
                    rc.Item().Text("DANFE — Documento Auxiliar da NF-e").FontSize(11).Bold().FontColor(cor);
                    rc.Item().Text(d.EmitenteRazaoSocial).Bold();
                    rc.Item().Text($"CNPJ: {d.EmitenteCnpj}").FontSize(8);
                });
                r.ConstantItem(150).AlignRight().Column(rc =>
                {
                    rc.Item().Text($"NF-e nº {d.NFe.Numero:D9}").Bold();
                    rc.Item().Text($"Série {d.NFe.Serie:D3}").FontSize(8);
                    rc.Item().Text(d.NFe.DataEmissao.ToString("dd/MM/yyyy")).FontSize(8);
                });
            });
            col.Item().PaddingTop(5).LineHorizontal(1).LineColor(cor);
            col.Item().PaddingTop(3).Text($"Chave de acesso: {d.NFe.ChaveAcesso}").FontSize(8).FontColor(Colors.Grey.Darken1);
            if (!string.IsNullOrWhiteSpace(d.NFe.ProtocoloAutorizacao))
                col.Item().Text($"Protocolo: {d.NFe.ProtocoloAutorizacao}  •  Autorizada em {d.NFe.DataAutorizacao:dd/MM/yyyy HH:mm}").FontSize(8).FontColor(Colors.Grey.Darken1);
        });
    }

    private static void Body(IContainer c, DanfeData d, string cor)
    {
        c.PaddingTop(10).Column(col =>
        {
            col.Item().Text("DESTINATÁRIO").Bold().FontColor(cor);
            col.Item().Text(d.ClienteNome);

            col.Item().PaddingTop(10).Text("PRODUTOS").Bold().FontColor(cor);
            col.Item().Table(t =>
            {
                t.ColumnsDefinition(c =>
                {
                    c.ConstantColumn(25);
                    c.RelativeColumn(3);
                    c.ConstantColumn(45);
                    c.ConstantColumn(60);
                    c.ConstantColumn(60);
                    c.ConstantColumn(70);
                });
                t.Header(h =>
                {
                    h.Cell().Text("#").Bold();
                    h.Cell().Text("Descrição").Bold();
                    h.Cell().Text("CFOP").Bold();
                    h.Cell().AlignRight().Text("Qtd").Bold();
                    h.Cell().AlignRight().Text("Vl Unit.").Bold();
                    h.Cell().AlignRight().Text("Total").Bold();
                });
                foreach (var item in d.Itens)
                {
                    t.Cell().Text(item.NumeroItem.ToString());
                    t.Cell().Text(item.Descricao);
                    t.Cell().Text(item.Cfop ?? "-");
                    t.Cell().AlignRight().Text(item.Quantidade.ToString("N2"));
                    t.Cell().AlignRight().Text(item.PrecoUnitario.ToString("C"));
                    t.Cell().AlignRight().Text(item.Total.ToString("C"));
                }
            });

            col.Item().PaddingTop(10).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);
            col.Item().PaddingTop(5).Row(r =>
            {
                r.RelativeItem().Text("VALOR TOTAL DA NOTA").Bold().FontColor(cor);
                r.ConstantItem(120).AlignRight().Text(d.NFe.ValorTotal.ToString("C")).Bold().FontColor(cor);
            });
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
