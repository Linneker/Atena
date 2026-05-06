using Acme.Sistemas.Services.V1.Relatorios.Pdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Acme.Sistemas.Infrastructure.Reports;

public sealed class QuestPdfPedidoCompraRenderer : IPedidoCompraPdfRenderer
{
    public byte[] Render(PedidoCompraPdfData data, TenantBranding branding)
    {
        var corPrimaria = ParseColor(branding.CorPrimariaHex, "#1F3A93");

        return Document.Create(doc =>
        {
            doc.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(t => t.FontSize(10));

                page.Header().Element(c => Header(c, branding, corPrimaria, data.Pedido.Numero));
                page.Content().Element(c => Body(c, data, corPrimaria));
                page.Footer().AlignCenter().Text(t =>
                {
                    t.Span("Pedido emitido em ").FontSize(8).FontColor(Colors.Grey.Medium);
                    t.Span(data.Pedido.DataEmissao.ToString("dd/MM/yyyy HH:mm")).FontSize(8).FontColor(Colors.Grey.Medium);
                });
            });
        }).GeneratePdf();
    }

    private static void Header(IContainer container, TenantBranding branding, string cor, string numero)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text(branding.RazaoSocial).FontSize(14).Bold().FontColor(cor);
                    c.Item().Text("PEDIDO DE COMPRA").FontSize(12).SemiBold();
                });
                row.ConstantItem(120).AlignRight().Column(c =>
                {
                    c.Item().Text("Nº").FontSize(8).FontColor(Colors.Grey.Medium);
                    c.Item().Text(numero).FontSize(14).Bold();
                });
            });
            col.Item().PaddingTop(8).LineHorizontal(1).LineColor(cor);
        });
    }

    private static void Body(IContainer container, PedidoCompraPdfData data, string cor)
    {
        container.PaddingTop(15).Column(col =>
        {
            col.Item().Row(r =>
            {
                r.RelativeItem().Column(c =>
                {
                    c.Item().Text("Fornecedor").FontSize(8).FontColor(Colors.Grey.Medium);
                    c.Item().Text(data.FornecedorNome).Bold();
                    if (!string.IsNullOrWhiteSpace(data.FornecedorEmail))
                        c.Item().Text(data.FornecedorEmail).FontSize(9);
                });
                r.RelativeItem().Column(c =>
                {
                    c.Item().Text("Emissão").FontSize(8).FontColor(Colors.Grey.Medium);
                    c.Item().Text(data.Pedido.DataEmissao.ToString("dd/MM/yyyy"));
                    if (data.Pedido.PrevisaoEntrega.HasValue)
                    {
                        c.Item().Text("Previsão entrega").FontSize(8).FontColor(Colors.Grey.Medium);
                        c.Item().Text(data.Pedido.PrevisaoEntrega.Value.ToString("dd/MM/yyyy"));
                    }
                    if (!string.IsNullOrWhiteSpace(data.Pedido.CondicaoPagamento))
                    {
                        c.Item().Text("Condição de pagamento").FontSize(8).FontColor(Colors.Grey.Medium);
                        c.Item().Text(data.Pedido.CondicaoPagamento);
                    }
                });
            });

            col.Item().PaddingTop(15).Text("ITENS").Bold().FontColor(cor);
            col.Item().PaddingTop(5).Table(table =>
            {
                table.ColumnsDefinition(c =>
                {
                    c.RelativeColumn(3); // Produto
                    c.RelativeColumn(1); // Qtd
                    c.RelativeColumn(1); // Preço Un
                    c.RelativeColumn(1); // Total
                });
                table.Header(h =>
                {
                    h.Cell().Text("Produto").Bold();
                    h.Cell().AlignRight().Text("Qtd").Bold();
                    h.Cell().AlignRight().Text("Preço Un.").Bold();
                    h.Cell().AlignRight().Text("Total").Bold();
                });
                foreach (var item in data.Itens)
                {
                    table.Cell().Text(item.ProdutoId.ToString().Substring(0, 8));
                    table.Cell().AlignRight().Text(item.Quantidade.ToString("N2"));
                    table.Cell().AlignRight().Text(item.PrecoUnitario.ToString("C"));
                    table.Cell().AlignRight().Text(item.Total.ToString("C"));
                }
            });

            col.Item().PaddingTop(10).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);
            col.Item().PaddingTop(5).Row(r =>
            {
                r.RelativeItem().Text("VALOR TOTAL").Bold().FontColor(cor);
                r.ConstantItem(120).AlignRight().Text(data.Pedido.ValorTotal.ToString("C")).Bold().FontColor(cor);
            });

            if (!string.IsNullOrWhiteSpace(data.Pedido.Observacao))
            {
                col.Item().PaddingTop(15).Text("Observações").Bold();
                col.Item().Text(data.Pedido.Observacao).FontSize(9);
            }
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
