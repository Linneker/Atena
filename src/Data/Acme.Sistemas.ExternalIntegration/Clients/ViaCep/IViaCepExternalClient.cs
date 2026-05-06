using Acme.Sistemas.ExternalIntegration.Helper;
using Acme.Sistemas.ExternalIntegration.Methods;

namespace Acme.Sistemas.ExternalIntegration.Clients.ViaCep;

public interface IViaCepExternalClient : IExternalApiClient
{
    [HttpGet("ws/{cep}/json")]
    Task<ApiResponse<ViaCepResponse>> ConsultarPorCepAsync(string cep);
}

public sealed record ViaCepResponse(
    string? Cep,
    string? Logradouro,
    string? Complemento,
    string? Bairro,
    string? Localidade,
    string? Uf,
    string? Ibge,
    string? Gia,
    string? Ddd,
    string? Siafi,
    bool? Erro);
