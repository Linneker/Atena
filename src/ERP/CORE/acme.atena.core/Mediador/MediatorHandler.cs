using acme.atena.core.Message;
using MediatR;

namespace acme.atena.core.Mediador
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> EnviarComando<T>(T comando) where T : Command
        {
            return await _mediator.Send(comando);
        }

        public async Task<Guid> EnviarComandoGuid<T>(T comando) where T : CommandGuid
        {
            return await _mediator.Send<Guid>(comando);
        }

        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
        }
    }
}
