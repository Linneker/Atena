namespace Acme.Sistemas.Services.V1.Despesa.Command.GerarRecorrencias;

public sealed record GerarRecorrenciasDespesaCommandResult(
    int Geradas,
    int IgnoradasJaExistentes);
