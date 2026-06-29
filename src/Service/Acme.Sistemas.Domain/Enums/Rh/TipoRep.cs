namespace Acme.Sistemas.Domain.Enums.Rh;

/// <summary>Tipo do Registrador Eletrônico de Ponto (Portaria MTP 671/2021).</summary>
public enum TipoRep
{
    /// <summary>REP-P — Programa instalado (desktop ou web local).</summary>
    RepP = 1,
    /// <summary>REP-C — Cloud (SaaS multi-tenant).</summary>
    RepC = 2,
}
