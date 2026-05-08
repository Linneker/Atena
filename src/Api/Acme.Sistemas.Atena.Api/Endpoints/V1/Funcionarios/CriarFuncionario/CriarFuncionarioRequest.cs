namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Funcionarios.CriarFuncionario;

public sealed record CriarFuncionarioRequest(
    string NomeCompleto,
    string Cpf,
    string? Email,
    string? Telefone,
    string? Cargo,
    string? Departamento,
    Guid? CentroDeCustoId,
    DateTime? DataAdmissao,
    Guid? UsuarioId);
