using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Engine;

/// <summary>
/// Implementação QuestPDF do espelho mensal. Inclui marca d'água "GERENCIAL —
/// NÃO SUBSTITUI PONTO OFICIAL PORTARIA 671" enquanto W4 (conformidade legal)
/// não está disponível.
/// </summary>
public sealed class GeradorEspelhoPdfQuestPdf : IGeradorEspelhoPdf
{
    static GeradorEspelhoPdfQuestPdf()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] Gerar(GeradorEspelhoMensal.EspelhoMensal e, string tenantRazaoSocial, string? logoUrl = null)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1.5f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(t => t.FontSize(9).FontFamily(Fonts.Calibri));

                page.Header().Column(col =>
                {
                    col.Item().Text($"ESPELHO DE PONTO — Competência {e.Competencia}").Bold().FontSize(14);
                    col.Item().Text(tenantRazaoSocial).FontSize(11);
                    col.Item().Text($"Funcionário: {e.FuncionarioNome} | CPF: {FormatarCpf(e.FuncionarioCpf)}");
                    col.Item().Text($"Jornada: {e.JornadaVigente.Nome} ({e.JornadaVigente.CargaSemanal}h/sem)");
                    col.Item().Text($"Política banco horas: {e.PoliticaBancoHoras?.Nome ?? "—"}");
                });

                page.Content().PaddingVertical(0.5f, Unit.Centimetre).Column(col =>
                {
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(2);  // data
                            c.RelativeColumn(1);  // dia
                            c.RelativeColumn(3);  // batidas
                            c.RelativeColumn(2);  // janela esperada
                            c.RelativeColumn(2);  // trabalhado
                            c.RelativeColumn(2);  // saldo
                        });
                        table.Header(h =>
                        {
                            h.Cell().Element(CellHeader).Text("Data");
                            h.Cell().Element(CellHeader).Text("Dia");
                            h.Cell().Element(CellHeader).Text("Batidas");
                            h.Cell().Element(CellHeader).Text("Esperada");
                            h.Cell().Element(CellHeader).Text("Trabalhado");
                            h.Cell().Element(CellHeader).Text("Saldo");
                        });

                        foreach (var d in e.Dias)
                        {
                            table.Cell().Element(Cell).Text(d.Data.ToString("dd/MM"));
                            table.Cell().Element(Cell).Text(d.DiaSemana + (d.EhFeriado ? " ★" : ""));
                            table.Cell().Element(Cell).Text(string.Join("  ", d.Batidas.Select(b => b.Hora)));
                            table.Cell().Element(Cell).Text(
                                d.JanelaEsperadaEntrada is null ? "—"
                                    : $"{d.JanelaEsperadaEntrada}–{d.JanelaEsperadaSaida}");
                            table.Cell().Element(Cell).Text(FormatarMin(d.TrabalhadoMinutos));
                            table.Cell().Element(Cell).Text(FormatarMinComSinal(d.SaldoMinutos));
                        }
                    });

                    col.Item().PaddingTop(0.5f, Unit.Centimetre).Column(t =>
                    {
                        t.Item().Text($"Trabalhado: {FormatarMin(e.Totais.TrabalhadoMinutos)} | " +
                                      $"Esperado: {FormatarMin(e.Totais.EsperadoMinutos)} | " +
                                      $"Saldo mês: {FormatarMinComSinal(e.Totais.SaldoMesMinutos)}").Bold();
                        t.Item().Text($"HE bruta: {FormatarMin(e.Totais.HorasExtrasMinutos)} | " +
                                      $"Saldo banco acumulado: {FormatarMinComSinal(e.Totais.SaldoBancoAcumuladoMinutos)}");
                        t.Item().Text($"Dias úteis: {e.Totais.DiasUteis} | " +
                                      $"Trabalhados: {e.Totais.DiasTrabalhados} | " +
                                      $"Faltas: {e.Totais.DiasFalta}");
                    });
                });

                page.Footer().Column(col =>
                {
                    col.Item().AlignCenter().Text("⚠ GERENCIAL — NÃO SUBSTITUI PONTO OFICIAL PORTARIA 671")
                        .Bold().FontColor(Colors.Red.Medium).FontSize(8);
                    col.Item().AlignCenter().Text($"Hash espelho: {e.HashEspelho[..16]}… | Gerado em: {e.GeradoEm:dd/MM/yyyy HH:mm} UTC")
                        .FontSize(7).FontColor(Colors.Grey.Darken1);
                });
            });
        }).GeneratePdf();
    }

    private static IContainer CellHeader(IContainer c) => c.Background(Colors.Grey.Lighten3)
        .PaddingVertical(3).PaddingHorizontal(4)
        .DefaultTextStyle(t => t.Bold().FontSize(8));

    private static IContainer Cell(IContainer c) => c.PaddingVertical(2).PaddingHorizontal(4)
        .BorderBottom(0.25f).BorderColor(Colors.Grey.Lighten2);

    private static string FormatarMin(int m) => m == 0 ? "—" : $"{m / 60:00}h{m % 60:00}";
    private static string FormatarMinComSinal(int m)
        => m == 0 ? "0h00" : (m > 0 ? "+" + FormatarMin(m) : "-" + FormatarMin(-m));

    private static string FormatarCpf(string cpf)
    {
        var d = new string(cpf.Where(char.IsDigit).ToArray());
        return d.Length == 11 ? $"{d[..3]}.{d[3..6]}.{d[6..9]}-{d[9..]}" : cpf;
    }
}
