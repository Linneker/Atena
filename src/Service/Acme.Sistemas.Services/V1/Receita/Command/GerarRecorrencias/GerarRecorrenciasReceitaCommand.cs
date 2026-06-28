using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Receita.Command.GerarRecorrencias;

public sealed record GerarRecorrenciasReceitaCommand(int Meses = 3)
    : IRequest<ResponseDefault<GerarRecorrenciasReceitaCommandResult>>;
