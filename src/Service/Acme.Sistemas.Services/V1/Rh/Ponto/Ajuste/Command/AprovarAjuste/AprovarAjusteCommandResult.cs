namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.AprovarAjuste;

public sealed record AprovarAjusteCommandResult(Guid AjusteId, Guid? MarcacaoResultanteId);
