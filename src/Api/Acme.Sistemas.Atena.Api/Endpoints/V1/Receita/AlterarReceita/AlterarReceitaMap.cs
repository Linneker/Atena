using Acme.Sistemas.Services.V1.Receita.Command.AlterarReceita;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.AlterarReceita;

public static class AlterarReceitaMap
{
    public static AlterarReceitaCommand ToCommand(this AlterarReceitaRequest request, Guid id)
        => new(
            id,
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

    public static AlterarReceitaResponse ToResponse(this AlterarReceitaCommandResult result)
        => new(result.Id);
}
