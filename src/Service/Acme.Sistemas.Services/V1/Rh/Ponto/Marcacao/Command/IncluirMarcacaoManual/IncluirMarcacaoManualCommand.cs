using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.IncluirMarcacaoManual;

/// <summary>
/// RH inclui batida manualmente (sempre auditada). Diferente de BaterPonto pois exige
/// funcionarioId no body (RH age em nome de outro funcionário).
/// </summary>
public sealed record IncluirMarcacaoManualCommand(
    Guid FuncionarioId,
    DateTime DataHora,
    TipoMarcacao Tipo,
    string Motivo) : IRequest<ResponseDefault<IncluirMarcacaoManualCommandResult>>;
