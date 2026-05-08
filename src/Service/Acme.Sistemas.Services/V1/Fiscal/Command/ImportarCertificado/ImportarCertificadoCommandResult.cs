using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.ImportarCertificado;

public sealed record ImportarCertificadoCommandResult(
    string Subject,
    DateTime ValidoAte,
    int DiasParaVencer);
