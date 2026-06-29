namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.ExportarAej;

public sealed record ExportarAejRequest(Guid EmpresaId, DateOnly PeriodoInicio, DateOnly PeriodoFim);
