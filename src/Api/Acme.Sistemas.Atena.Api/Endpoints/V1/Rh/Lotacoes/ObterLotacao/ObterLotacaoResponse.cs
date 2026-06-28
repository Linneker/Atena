namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Lotacoes.ObterLotacao;

public sealed record ObterLotacaoResponse(
    Guid Id,
    string Nome,
    Guid? EmpresaId,
    string? Cnpj,
    string? EnderecoJson,
    bool Ativo);
