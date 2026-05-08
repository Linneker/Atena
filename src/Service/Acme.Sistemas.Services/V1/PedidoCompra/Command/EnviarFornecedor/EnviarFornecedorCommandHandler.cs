using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Domain.Interfaces.Messaging;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Compras;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Reports;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Command.EnviarFornecedor;

public sealed class EnviarFornecedorCommandHandler
    : IRequestHandler<EnviarFornecedorCommand, ResponseDefault<EnviarFornecedorCommandResult>>
{
    private readonly IPedidoCompraRepository _pedidos;
    private readonly IFornecedorRepository _fornecedores;
    private readonly ITenantRepository _tenants;
    private readonly ITenantContext _tenantContext;
    private readonly IPedidoCompraPdfRenderer _pdf;
    private readonly IEmailQueueService _emails;

    public EnviarFornecedorCommandHandler(
        IPedidoCompraRepository pedidos,
        IFornecedorRepository fornecedores,
        ITenantRepository tenants,
        ITenantContext tenantContext,
        IPedidoCompraPdfRenderer pdf,
        IEmailQueueService emails)
    {
        _pedidos = pedidos;
        _fornecedores = fornecedores;
        _tenants = tenants;
        _tenantContext = tenantContext;
        _pdf = pdf;
        _emails = emails;
    }

    public async Task<ResponseDefault<EnviarFornecedorCommandResult>> Handle(EnviarFornecedorCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidos.GetByIdAsync(request.PedidoId, cancellationToken);
        if (pedido is null)
            return ResponseDefault<EnviarFornecedorCommandResult>.NotFound("Pedido de compra não encontrado.");

        if (pedido.Status == StatusPedidoCompra.Cancelado)
            return ResponseDefault<EnviarFornecedorCommandResult>.Conflict("Pedido cancelado.");
        if (pedido.Status == StatusPedidoCompra.Recebido)
            return ResponseDefault<EnviarFornecedorCommandResult>.Conflict("Pedido já recebido.");

        var fornecedor = await _fornecedores.GetByIdAsync(pedido.FornecedorId, cancellationToken);
        if (fornecedor is null)
            return ResponseDefault<EnviarFornecedorCommandResult>.NotFound("Fornecedor não encontrado.");

        var emailDestino = request.EmailDestinoOverride ?? fornecedor.Email;
        if (string.IsNullOrWhiteSpace(emailDestino))
            return ResponseDefault<EnviarFornecedorCommandResult>.BadRequest(
                Core.Response.Erros.Error.Validation("Fornecedor sem e-mail cadastrado e nenhum override informado."));

        var itens = await _pedidos.ListItensAsync(pedido.Id, cancellationToken);

        var tenant = await _tenants.GetByIdAsync(_tenantContext.TenantId, cancellationToken);
        var branding = new TenantBranding(
            tenant?.RazaoSocial ?? "Atena",
            tenant?.LogoUrl,
            tenant?.CorPrimaria);

        var pdfBytes = _pdf.Render(
            new PedidoCompraPdfData(pedido, itens, fornecedor.Nome, fornecedor.Email),
            branding);

        var body = $@"<p>Prezado {System.Net.WebUtility.HtmlEncode(fornecedor.Nome)},</p>
<p>Segue em anexo o pedido de compra <strong>{pedido.Numero}</strong> emitido em {pedido.DataEmissao:dd/MM/yyyy}.</p>
<p>Valor total: <strong>{pedido.ValorTotal:C}</strong></p>
<p>Atenciosamente,<br/>{System.Net.WebUtility.HtmlEncode(branding.RazaoSocial)}</p>";

        await _emails.EnqueueAsync(new EmailMessage(
            To: emailDestino!,
            Subject: $"Pedido de compra {pedido.Numero} - {branding.RazaoSocial}",
            Body: body,
            IsHtml: true,
            Attachments: new[]
            {
                new EmailAttachment($"pedido-{pedido.Numero}.pdf", pdfBytes, "application/pdf")
            }), cancellationToken);

        await _pedidos.UpdateStatusAsync(pedido.Id, StatusPedidoCompra.EnviadoFornecedor, cancellationToken);

        return ResponseDefault<EnviarFornecedorCommandResult>.Ok(
            new EnviarFornecedorCommandResult(pedido.Id, emailDestino!, DateTime.UtcNow));
    }
}
