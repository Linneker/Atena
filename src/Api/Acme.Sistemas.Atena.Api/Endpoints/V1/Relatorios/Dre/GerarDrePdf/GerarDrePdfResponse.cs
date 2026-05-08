namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Dre.GerarDrePdf;

// Endpoint retorna application/pdf — Response é stream binário (FileResult).
// Tipo definido como marker para manter consistência do padrão.
public sealed record GerarDrePdfResponse;
