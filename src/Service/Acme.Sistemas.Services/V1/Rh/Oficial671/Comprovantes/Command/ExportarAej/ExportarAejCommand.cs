using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Command.ExportarAej;

public sealed record ExportarAejCommand(
    Guid EmpresaId,
    DateOnly PeriodoInicio,
    DateOnly PeriodoFim) : IRequest<ResponseDefault<ExportarAejCommandResult>>;
