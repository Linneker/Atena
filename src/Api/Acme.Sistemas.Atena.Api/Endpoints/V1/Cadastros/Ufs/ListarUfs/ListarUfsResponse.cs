namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Cadastros.Ufs.ListarUfs;

public sealed record ListarUfsResponseItem(string Sigla, string Nome, int CodigoIbge);

public sealed record ListarUfsResponse(IReadOnlyList<ListarUfsResponseItem> Items);
