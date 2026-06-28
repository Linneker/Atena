using System.Text;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Email;

/// <summary>
/// Templates HTML simples para os 3 e-mails do fluxo de ponto (ajuste aprovado/rejeitado,
/// espelho disponível, digest de pendentes ao gestor). Interpolação via parâmetros tipados.
/// Templates renderizam para HTML básico responsivo (compatível com Gmail/Outlook).
/// </summary>
public static class PontoEmailTemplates
{
    private const string HeaderHtml = """
        <div style="font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; max-width:600px; margin:0 auto; background:#fff;">
          <div style="background:#1d4ed8; color:#fff; padding:16px;">
            <h2 style="margin:0; font-size:18px;">Atena ERP — Ponto</h2>
          </div>
          <div style="padding:24px; color:#111;">
        """;

    private const string FooterHtml = """
          </div>
          <div style="padding:12px 24px; color:#888; font-size:11px; border-top:1px solid #eee;">
            Esta é uma notificação automática do sistema Atena.
            Acesse <a href="https://app.atena.local/rh/ponto/meu-ponto">/rh/ponto/meu-ponto</a> para detalhes.
          </div>
        </div>
        """;

    // ============================== 1. Ajuste decidido ==============================

    /// <summary>Template "Sua solicitação de ajuste foi aprovada (ou rejeitada)".</summary>
    public static EmailRender AjusteDecidido(string nomeDestinatario, bool aprovado, string motivoSolicitacao,
        string? justificativaDecisao, string nomeAprovador, DateTime decisaoEm)
    {
        var statusTexto = aprovado ? "APROVADA" : "REJEITADA";
        var statusCor = aprovado ? "#16a34a" : "#dc2626";

        var assunto = aprovado
            ? "[Atena] Sua solicitação de ajuste de ponto foi aprovada"
            : "[Atena] Sua solicitação de ajuste de ponto foi rejeitada";

        var html = new StringBuilder(HeaderHtml)
            .Append("<p>Olá, <strong>").Append(EscapeHtml(nomeDestinatario)).Append("</strong>!</p>")
            .Append("<p>Sua solicitação de ajuste de ponto foi <strong style=\"color:")
            .Append(statusCor).Append("\">").Append(statusTexto).Append("</strong>.</p>")
            .Append("<div style=\"background:#f4f6f8; border-left:4px solid ").Append(statusCor)
            .Append("; padding:12px 16px; margin:16px 0;\">")
            .Append("<p style=\"margin:0 0 8px 0;\"><strong>Sua solicitação:</strong></p>")
            .Append("<p style=\"margin:0; font-style:italic; color:#555;\">").Append(EscapeHtml(motivoSolicitacao)).Append("</p>")
            .Append("</div>")
            .Append("<p><strong>Decidido por:</strong> ").Append(EscapeHtml(nomeAprovador))
            .Append(" em ").Append(decisaoEm.ToString("dd/MM/yyyy HH:mm")).Append(" UTC</p>");

        if (!string.IsNullOrWhiteSpace(justificativaDecisao))
        {
            html.Append("<p><strong>Justificativa:</strong></p>")
                .Append("<p style=\"background:#fff8e1; padding:12px; border-radius:4px;\">")
                .Append(EscapeHtml(justificativaDecisao)).Append("</p>");
        }

        html.Append(FooterHtml);

        var texto = $"Olá, {nomeDestinatario}!\n\n" +
                    $"Sua solicitação de ajuste de ponto foi {statusTexto}.\n\n" +
                    $"Sua solicitação:\n{motivoSolicitacao}\n\n" +
                    $"Decidido por: {nomeAprovador} em {decisaoEm:dd/MM/yyyy HH:mm} UTC\n" +
                    (justificativaDecisao is null ? "" : $"Justificativa: {justificativaDecisao}\n");

        return new EmailRender(assunto, html.ToString(), texto);
    }

    // ============================== 2. Espelho disponível ============================

    /// <summary>Template "Seu espelho mensal está disponível".</summary>
    public static EmailRender EspelhoDisponivel(string nomeDestinatario, string competencia,
        string urlEspelho, int trabalhadoMinutos, int saldoMesMinutos)
    {
        var assunto = $"[Atena] Espelho de ponto {competencia} disponível";
        var saldoCor = saldoMesMinutos >= 0 ? "#16a34a" : "#dc2626";
        var saldoTexto = saldoMesMinutos >= 0 ? "+" + FormatMin(saldoMesMinutos) : "-" + FormatMin(-saldoMesMinutos);

        var html = new StringBuilder(HeaderHtml)
            .Append("<p>Olá, <strong>").Append(EscapeHtml(nomeDestinatario)).Append("</strong>!</p>")
            .Append("<p>Seu espelho de ponto da competência <strong>").Append(competencia)
            .Append("</strong> está disponível.</p>")
            .Append("<table style=\"border-collapse:collapse; margin:16px 0;\">")
            .Append("<tr><td style=\"padding:8px 16px; background:#f4f6f8;\">Trabalhado</td><td style=\"padding:8px 16px;\"><strong>").Append(FormatMin(trabalhadoMinutos)).Append("</strong></td></tr>")
            .Append("<tr><td style=\"padding:8px 16px; background:#f4f6f8;\">Saldo do mês</td><td style=\"padding:8px 16px; color:")
            .Append(saldoCor).Append(";\"><strong>").Append(saldoTexto).Append("</strong></td></tr>")
            .Append("</table>")
            .Append("<p><a href=\"").Append(urlEspelho).Append("\" style=\"display:inline-block; background:#1d4ed8; color:#fff; padding:10px 16px; text-decoration:none; border-radius:4px;\">Ver espelho completo</a></p>")
            .Append(FooterHtml);

        var texto = $"Olá, {nomeDestinatario}!\n\n" +
                    $"Seu espelho de ponto da competência {competencia} está disponível.\n\n" +
                    $"Trabalhado: {FormatMin(trabalhadoMinutos)}\n" +
                    $"Saldo do mês: {saldoTexto}\n\n" +
                    $"Acesse: {urlEspelho}\n";

        return new EmailRender(assunto, html.ToString(), texto);
    }

    // ============================== 3. Digest gestor =================================

    /// <summary>Template digest diário para o gestor — lista ajustes pendentes da equipe.</summary>
    public static EmailRender DigestPendentesGestor(string nomeGestor, int totalPendentes,
        IReadOnlyList<DigestItem> itens, string urlAprovacoes)
    {
        var assunto = $"[Atena] Você tem {totalPendentes} ajuste(s) de ponto para aprovar";

        var html = new StringBuilder(HeaderHtml)
            .Append("<p>Olá, <strong>").Append(EscapeHtml(nomeGestor)).Append("</strong>!</p>")
            .Append("<p>Você tem <strong>").Append(totalPendentes)
            .Append("</strong> ajuste(s) de ponto pendente(s) de aprovação.</p>")
            .Append("<table style=\"border-collapse:collapse; width:100%; margin:16px 0; font-size:13px;\">")
            .Append("<tr style=\"background:#f4f6f8;\"><th style=\"padding:8px; text-align:left;\">Funcionário</th><th style=\"padding:8px; text-align:left;\">Tipo</th><th style=\"padding:8px; text-align:left;\">Motivo</th><th style=\"padding:8px; text-align:left;\">Solicitado</th></tr>");

        foreach (var i in itens.Take(10))
        {
            html.Append("<tr><td style=\"padding:6px 8px; border-bottom:1px solid #eee;\">")
                .Append(EscapeHtml(i.NomeFuncionario)).Append("</td>")
                .Append("<td style=\"padding:6px 8px; border-bottom:1px solid #eee;\">").Append(i.Tipo).Append("</td>")
                .Append("<td style=\"padding:6px 8px; border-bottom:1px solid #eee;\">").Append(EscapeHtml(i.Motivo)).Append("</td>")
                .Append("<td style=\"padding:6px 8px; border-bottom:1px solid #eee;\">").Append(i.SolicitadoEm.ToString("dd/MM HH:mm")).Append("</td></tr>");
        }
        html.Append("</table>");

        if (itens.Count > 10)
            html.Append("<p style=\"color:#888; font-size:12px;\">… e mais ").Append(itens.Count - 10).Append(" outros.</p>");

        html.Append("<p><a href=\"").Append(urlAprovacoes)
            .Append("\" style=\"display:inline-block; background:#1d4ed8; color:#fff; padding:10px 16px; text-decoration:none; border-radius:4px;\">Ir para aprovações</a></p>")
            .Append(FooterHtml);

        var texto = new StringBuilder()
            .Append($"Olá, {nomeGestor}!\n\n")
            .Append($"Você tem {totalPendentes} ajuste(s) de ponto pendente(s) de aprovação.\n\n")
            .Append("Resumo:\n");

        foreach (var i in itens.Take(10))
            texto.Append($"  - {i.NomeFuncionario} | {i.Tipo} | {i.SolicitadoEm:dd/MM HH:mm} | {i.Motivo}\n");

        texto.Append($"\nAcesse: {urlAprovacoes}\n");

        return new EmailRender(assunto, html.ToString(), texto.ToString());
    }

    private static string FormatMin(int m)
        => $"{m / 60:00}h{m % 60:00}";

    private static string EscapeHtml(string s)
        => System.Net.WebUtility.HtmlEncode(s);
}

public sealed record DigestItem(string NomeFuncionario, string Tipo, string Motivo, DateTime SolicitadoEm);

public sealed record EmailRender(string Assunto, string Html, string Texto);
