using System.Globalization;
using Acme.Sistemas.Domain.Interfaces.Rh;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Servicos;

/// <summary>
/// PDF do comprovante de marcação Portaria 671/2021. Layout simples uma página A4
/// (texto plano para máxima legibilidade do auditor / impressora térmica de bolso).
/// QR code opcional aponta para URL pública de verificação (off-band).
/// </summary>
public sealed class GeradorComprovantePontoPdf : IGeradorComprovantePontoPdf
{
    public GeradorComprovantePontoPdf()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] Gerar(DadosComprovantePdf d)
    {
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(s => s.FontSize(11).FontFamily("Helvetica"));

                page.Header().Column(col =>
                {
                    col.Item().Text(d.RazaoSocialEmpregador).FontSize(14).Bold();
                    col.Item().Text($"CNPJ: {FormatarCnpj(d.CnpjEmpregador)}");
                    if (!string.IsNullOrWhiteSpace(d.EnderecoEmpregador))
                        col.Item().Text(d.EnderecoEmpregador).FontSize(9);
                });

                page.Content().PaddingVertical(20).Column(col =>
                {
                    col.Spacing(8);
                    col.Item().Text("COMPROVANTE DE REGISTRO DE PONTO").FontSize(13).Bold();
                    col.Item().Text("Portaria MTP 671/2021 — anexo II").FontSize(9).Italic();
                    col.Item().LineHorizontal(0.5f);

                    col.Item().Text(t =>
                    {
                        t.Span("Empregado: ").Bold();
                        t.Span(d.NomeEmpregado);
                    });
                    col.Item().Text(t =>
                    {
                        t.Span("CPF: ").Bold();
                        t.Span(FormatarCpf(d.CpfEmpregado));
                        t.Span("    PIS: ").Bold();
                        t.Span(FormatarPis(d.PisEmpregado));
                    });
                    col.Item().Text(t =>
                    {
                        t.Span("Data: ").Bold();
                        t.Span(d.DataHora.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                        t.Span("    Hora: ").Bold();
                        t.Span(d.DataHora.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
                    });
                    col.Item().Text(t =>
                    {
                        t.Span("Tipo: ").Bold();
                        t.Span(d.TipoRegistro);
                    });
                    col.Item().Text(t =>
                    {
                        t.Span("NSR: ").Bold();
                        t.Span(d.Nsr.ToString("D9", CultureInfo.InvariantCulture));
                    });

                    col.Item().PaddingTop(10).LineHorizontal(0.5f);
                    col.Item().Text("Assinatura digital ICP-Brasil (RSA-SHA-256):").Bold();
                    col.Item().Text(Truncar(d.AssinaturaResumoBase64, 256)).FontFamily("Courier").FontSize(7);

                    col.Item().Text(t =>
                    {
                        t.Span("Hash SHA-256: ").Bold();
                        t.Span(d.HashSha256Hex).FontFamily("Courier").FontSize(8);
                    });

                    if (!string.IsNullOrWhiteSpace(d.QrCodeUrlVerificacao))
                    {
                        col.Item().PaddingTop(10).Text(t =>
                        {
                            t.Span("Verificar comprovante: ").Bold();
                            t.Span(d.QrCodeUrlVerificacao!).FontSize(8);
                        });
                    }
                });

                page.Footer().AlignCenter().Text(t =>
                {
                    t.Span("Comprovante gerado pelo REP — não rasure. ").FontSize(8);
                    t.Span("Lei nº 13.874/2019 + Portaria MTP 671/2021.").FontSize(8).Italic();
                });
            });
        });
        return doc.GeneratePdf();
    }

    private static string FormatarCnpj(string c)
    {
        var d = new string(c.Where(char.IsDigit).ToArray());
        return d.Length == 14 ? $"{d[..2]}.{d[2..5]}.{d[5..8]}/{d[8..12]}-{d[12..]}" : c;
    }
    private static string FormatarCpf(string c)
    {
        var d = new string(c.Where(char.IsDigit).ToArray());
        return d.Length == 11 ? $"{d[..3]}.{d[3..6]}.{d[6..9]}-{d[9..]}" : c;
    }
    private static string FormatarPis(string c)
    {
        var d = new string(c.Where(char.IsDigit).ToArray());
        return d.Length == 11 ? $"{d[..3]}.{d[3..8]}.{d[8..10]}-{d[10..]}" : c;
    }
    private static string Truncar(string s, int max) => s.Length <= max ? s : s[..max] + "…";
}
