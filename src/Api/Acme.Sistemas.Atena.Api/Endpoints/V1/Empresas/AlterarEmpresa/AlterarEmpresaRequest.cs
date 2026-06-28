using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Empresas.AlterarEmpresa;

public sealed record AlterarEmpresaRequest(
    string RazaoSocial,
    string? NomeFantasia,
    string Cnpj,
    string? InscricaoEstadual,
    string? InscricaoMunicipal,
    string? Email,
    string? Telefone,
    StatusAtivo Status,
    EnderecoDto? Endereco,
    bool BuscarEnderecoPorCep = false);
