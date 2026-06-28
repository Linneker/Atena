namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Lotacoes.CriarLotacao;

public sealed record CriarLotacaoRequest(
    string Nome,
    Guid? EmpresaId,
    string? Cnpj,
    string? EnderecoJson);
