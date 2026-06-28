namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Lotacoes.AlterarLotacao;

public sealed record AlterarLotacaoRequest(
    Guid Id,
    string Nome,
    Guid? EmpresaId,
    string? Cnpj,
    string? EnderecoJson,
    bool Ativo);
