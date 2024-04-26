using acme.atena.core.Message;

namespace acme.atena.core.Mediador
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<bool> EnviarComando<T>(T comando) where T : Command;

        Task<Guid> EnviarComandoGuid<T>(T comando) where T : CommandGuid;

    }
}
