namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.VincularBeneficio;

public sealed record VincularBeneficioRequest(
    Guid FuncionarioId,
    Guid BeneficioCatalogoId,
    decimal? Valor,
    decimal? DescontoFuncionarioPct,
    DateOnly VigenciaInicio,
    string? Observacao);
