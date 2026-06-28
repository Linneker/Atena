using Acme.Sistemas.Services.V1.Rh.Funcionario.Query.ObterFichaCompleta;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.ObterFichaCompleta;

// Alias do Result do Query — DTOs já são imutáveis e seguros para serializar.
public sealed record ObterFichaCompletaResponse(ObterFichaCompletaQueryResult Ficha);
