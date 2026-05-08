using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.DevolucaoVenda.Command.RegistrarDevolucao;

public sealed record DevolucaoItemDto(Guid FaturamentoItemId, decimal Quantidade);

