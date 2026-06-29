namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.ExportarAfd;

public sealed record ExportarAfdRequest(Guid EmpresaId, DateOnly PeriodoInicio, DateOnly PeriodoFim);
