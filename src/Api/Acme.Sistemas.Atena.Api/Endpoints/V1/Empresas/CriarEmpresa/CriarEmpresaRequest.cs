using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Empresas.CriarEmpresa;

public sealed record CriarEmpresaRequest(
    string RazaoSocial,
    string? NomeFantasia,
    string Cnpj,
    string? InscricaoEstadual,
    string? InscricaoMunicipal,
    string? Email,
    string? Telefone,
    EnderecoDto? Endereco,
    bool BuscarEnderecoPorCep = false);
