using Acme.Sistemas.Domain.Enums.Rh;

namespace Acme.Sistemas.Domain.Entities.Rh;

/// <summary>
/// Rubrica customizável do tenant. Cada tenant define suas próprias rubricas em DSL minimalista,
/// pode clonar do catálogo nacional ou criar do zero. Folha (W6) consome rubricas ativas com
/// vigência válida na competência do cálculo, em ordem topológica conforme <c>DependenciasJson</c>.
/// </summary>
public sealed class RubricaTenant : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public TipoRubrica Tipo { get; set; }
    public string? NaturezaEsocialCodigo { get; set; }
    public string FormulaDsl { get; set; } = string.Empty;
    public bool IncideInss { get; set; }
    public bool IncideIrrf { get; set; }
    public bool IncideFgts { get; set; }
    public bool IncideFerias { get; set; }
    public bool Incide13o { get; set; }
    public bool IncideDsr { get; set; }
    public string? DependenciasJson { get; set; }
    public DateOnly VigenciaInicio { get; set; }
    public DateOnly? VigenciaFim { get; set; }
    public bool Ativa { get; set; } = true;
    public OrigemRubrica Origem { get; set; } = OrigemRubrica.Custom;
    public string? CodigoCatalogoOrigem { get; set; }
}
