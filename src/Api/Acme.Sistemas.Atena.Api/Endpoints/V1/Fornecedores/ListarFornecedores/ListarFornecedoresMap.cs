using Acme.Sistemas.Services.V1.Fornecedor.Query.ListarFornecedores;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fornecedores.ListarFornecedores;

public static class ListarFornecedoresMap
{
    public static ListarFornecedoresQuery ToQuery(this ListarFornecedoresRequest request)
        => new(request.Termo, request.Skip ?? 0, request.Take ?? 50);

    public static ListarFornecedoresResponse ToResponse(this ListarFornecedoresQueryResult result)
        => new(result.Items.Select(i => new ListarFornecedoresResponseItem(
            i.Id, i.Tipo, i.Nome, i.NomeFantasia, i.Documento, i.Email,
            i.Telefone, i.CondicaoPagamentoPadrao, i.Status)).ToArray(),
            result.Total);
}
