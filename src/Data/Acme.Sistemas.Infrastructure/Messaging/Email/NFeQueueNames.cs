namespace Acme.Sistemas.Infrastructure.Messaging.Email;

public static class NFeQueueNames
{
    public const string Exchange = "atena.nfe";
    public const string Queue = "atena.nfe.transmissao";
    public const string RoutingKey = "nfe.transmitir";

    public const string DeadLetterExchange = "atena.nfe.dlx";
    public const string DeadLetterQueue = "atena.nfe.dlq";
    public const string DeadLetterRoutingKey = "nfe.transmitir.dead";
}

public sealed record NFeTransmissaoMessage(Guid TenantId, Guid NFeId);
