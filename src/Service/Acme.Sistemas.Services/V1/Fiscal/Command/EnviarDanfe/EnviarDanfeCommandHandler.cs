using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Domain.Interfaces.Messaging;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Reports;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.EnviarDanfe;

public sealed class EnviarDanfeCommandHandler
    : IRequestHandler<EnviarDanfeCommand, ResponseDefault<EnviarDanfeCommandResult>>
{
    private readonly INFeRepository _nfes;
    private readonly IClienteRepository _clientes;
    private readonly ITenantRepository _tenants;
    private readonly IConfiguracaoFiscalRepository _config;
    private readonly IDanfePdfRenderer _pdf;
    private readonly IEmailQueueService _emails;
    private readonly ITenantContext _tenantContext;

    public EnviarDanfeCommandHandler(
        INFeRepository nfes,
        IClienteRepository clientes,
        ITenantRepository tenants,
        IConfiguracaoFiscalRepository config,
        IDanfePdfRenderer pdf,
        IEmailQueueService emails,
        ITenantContext tenantContext)
    {
        _nfes = nfes;
        _clientes = clientes;
        _tenants = tenants;
        _config = config;
        _pdf = pdf;
        _emails = emails;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<EnviarDanfeCommandResult>> Handle(EnviarDanfeCommand request, CancellationToken cancellationToken)
    {
        var nfe = await _nfes.GetByIdAsync(request.NFeId, cancellationToken);
        if (nfe is null)
            return ResponseDefault<EnviarDanfeCommandResult>.NotFound("NF-e não encontrada.");
        if (nfe.Status != StatusNFe.Autorizada)
            return ResponseDefault<EnviarDanfeCommandResult>.Conflict(
                $"DANFE só pode ser enviada para NF-e autorizada (status atual: {nfe.Status}).");

        var cliente = await _clientes.GetByIdAsync(nfe.ClienteId, cancellationToken);
        var emailDestino = request.EmailDestinoOverride ?? cliente?.Email;
        if (string.IsNullOrWhiteSpace(emailDestino))
            return ResponseDefault<EnviarDanfeCommandResult>.BadRequest(
                Core.Response.Erros.Error.Validation("Cliente sem e-mail e nenhum override informado."));

        var itens = await _nfes.ListItensAsync(nfe.Id, cancellationToken);
        var config = await _config.GetAsync(cancellationToken);
        var tenant = await _tenants.GetByIdAsync(_tenantContext.TenantId, cancellationToken);

        var branding = new TenantBranding(
            tenant?.RazaoSocial ?? "Atena",
            tenant?.LogoUrl,
            tenant?.CorPrimaria);

        var pdfBytes = _pdf.Render(
            new DanfeData(nfe, itens,
                config?.RazaoSocialEmitente ?? branding.RazaoSocial,
                config?.CnpjEmitente ?? string.Empty,
                cliente?.Nome ?? "Cliente"),
            branding);

        var body = $@"<p>Olá {System.Net.WebUtility.HtmlEncode(cliente?.Nome ?? "")},</p>
<p>Segue em anexo o DANFE da NF-e número <strong>{nfe.Numero:D9}</strong>, série {nfe.Serie:D3}.</p>
<p>Chave de acesso: {nfe.ChaveAcesso}<br/>
Valor total: <strong>{nfe.ValorTotal:C}</strong></p>
<p>Atenciosamente,<br/>{System.Net.WebUtility.HtmlEncode(branding.RazaoSocial)}</p>";

        await _emails.EnqueueAsync(new EmailMessage(
            To: emailDestino!,
            Subject: $"DANFE NF-e {nfe.Numero:D9} — {branding.RazaoSocial}",
            Body: body,
            IsHtml: true,
            Attachments: new[]
            {
                new EmailAttachment($"danfe-{nfe.ChaveAcesso}.pdf", pdfBytes, "application/pdf")
            }), cancellationToken);

        return ResponseDefault<EnviarDanfeCommandResult>.Ok(
            new EnviarDanfeCommandResult(nfe.Id, emailDestino!));
    }
}
