using Acme.Sistemas.Services.V1.Despesa.Command.CriarDespesa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.CriarDespesa;

public static class CriarDespesaMap
{
    public static CriarDespesaCommand ToCommand(this CriarDespesaRequest request)
        => new(
            request.Nome,
            request.Descricao,
            request.Categoria,
            request.Valor,
            request.DespesaFixa,
            request.DataVencimento,
            request.CompetenciaId,
            request.CentroDeCustoId,
            request.FornecedorId);

    public static CriarDespesaResponse ToResponse(this CriarDespesaCommandResult result)
        => new(result.Id, result.Nome, result.Valor, result.DataVencimento);
}
