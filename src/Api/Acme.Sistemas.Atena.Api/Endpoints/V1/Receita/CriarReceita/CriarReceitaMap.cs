using Acme.Sistemas.Services.V1.Receita.Command.CriarReceita;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.CriarReceita;

public static class CriarReceitaMap
{
    public static CriarReceitaCommand ToCommand(this CriarReceitaRequest request)
        => new(
            request.Nome,
            request.Descricao,
            request.Categoria,
            request.Valor,
            request.ReceitaFixa,
            request.DataPrevistaRecebimento,
            request.CompetenciaId,
            request.CentroDeCustoId,
            request.ClienteId,
            request.OrigemVendaId);

    public static CriarReceitaResponse ToResponse(this CriarReceitaCommandResult result)
        => new(result.Id, result.Nome, result.Valor, result.DataPrevistaRecebimento);
}
