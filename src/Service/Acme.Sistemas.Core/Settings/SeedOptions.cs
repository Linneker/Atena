namespace Acme.Sistemas.Core.Settings;

/// <summary>
/// Configurações de bootstrap/seed de dados (estáticos BR + tenant demo).
/// </summary>
public sealed class SeedOptions
{
    public const string SectionName = "Seed";

    /// <summary>
    /// Em ambiente Development, se true e não houver tenant, cria <c>demo@atena.test</c> no boot.
    /// Ignorado em Production (proteção dupla).
    /// </summary>
    public bool AutoBootstrap { get; set; }

    /// <summary>Carrega ~10k NCMs no boot (dataset pesado, opt-in). Default false.</summary>
    public bool LoadNcmsOnStartup { get; set; }

    /// <summary>Carrega ~5570 municípios IBGE no boot (dataset pesado, opt-in). Default false.</summary>
    public bool LoadMunicipiosOnStartup { get; set; }
}
