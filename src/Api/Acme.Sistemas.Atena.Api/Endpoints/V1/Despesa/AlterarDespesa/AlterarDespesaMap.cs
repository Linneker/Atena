using Acme.Sistemas.Services.V1.Despesa.Command.AlterarDespesa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.AlterarDespesa;

public static class AlterarDespesaMap
{
    public static AlterarDespesaCommand ToCommand(this AlterarDespesaRequest request, Guid id)
        => new(
            id,
            request.Nome,
            request.Descricao,
            request.Categoria,
            request.Valor,
            request.DespesaFixa,
            request.DataVencimento,
            request.CompetenciaId,
            request.CentroDeCustoId,
            request.FornecedorId);

    public static AlterarDespesaResponse ToResponse(this AlterarDespesaCommandResult result)
        => new(result.Id);
}
