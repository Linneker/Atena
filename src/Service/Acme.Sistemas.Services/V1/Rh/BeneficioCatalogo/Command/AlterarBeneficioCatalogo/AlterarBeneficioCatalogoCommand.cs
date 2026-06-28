using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Command.AlterarBeneficioCatalogo;

public sealed record AlterarBeneficioCatalogoCommand(
    Guid Id,
    string? Codigo,
    string Descricao,
    TipoBeneficio Tipo,
    decimal? DescontoFuncionarioPct,
    decimal? CustoEmpresaPadrao,
    string? NaturezaRubricaEsocial,
    bool Ativo) : IRequest<ResponseDefault<AlterarBeneficioCatalogoCommandResult>>;
