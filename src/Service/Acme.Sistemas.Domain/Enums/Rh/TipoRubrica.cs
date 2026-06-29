namespace Acme.Sistemas.Domain.Enums.Rh;

/// <summary>
/// Classifica a rubrica para o cálculo de folha. <c>Provento</c> soma à remuneração,
/// <c>Desconto</c> reduz, <c>Informativa</c> apenas registra sem afetar líquido.
/// </summary>
public enum TipoRubrica
{
    Provento = 1,
    Desconto = 2,
    Informativa = 3,
}
