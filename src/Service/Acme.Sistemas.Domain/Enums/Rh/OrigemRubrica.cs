namespace Acme.Sistemas.Domain.Enums.Rh;

/// <summary>
/// De onde veio a rubrica do tenant. <c>CatalogoClonada</c> = veio do
/// <c>rubricas_catalogo_nacional</c>; <c>Custom</c> = criada do zero pelo tenant;
/// <c>BuiltIn</c> = semeada no provisionamento de tenant (default vigente).
/// </summary>
public enum OrigemRubrica
{
    CatalogoClonada = 1,
    Custom = 2,
    BuiltIn = 3,
}
