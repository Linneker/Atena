namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dashboard.ObterKpis;

public sealed record ObterKpisRequest(DateTime? Inicio = null, DateTime? Fim = null);
