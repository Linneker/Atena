namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Empresas.CriarEmpresa;

public sealed record CriarEmpresaResponse(
    Guid Id,
    string RazaoSocial,
    string Cnpj);
