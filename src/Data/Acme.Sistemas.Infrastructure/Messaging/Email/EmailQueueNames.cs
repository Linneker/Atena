namespace Acme.Sistemas.Infrastructure.Messaging.Email;

public static class EmailQueueNames
{
    public const string Exchange = "atena.email";
    public const string Queue = "atena.email";
    public const string RoutingKey = "email.send";

    public const string DeadLetterExchange = "atena.email.dlx";
    public const string DeadLetterQueue = "atena.email.dlq";
    public const string DeadLetterRoutingKey = "email.send.dead";
}
