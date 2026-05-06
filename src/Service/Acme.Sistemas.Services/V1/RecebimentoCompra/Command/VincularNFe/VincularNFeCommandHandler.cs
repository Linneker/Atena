using Acme.Sistemas.Core.Helper;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.RecebimentoCompra.Command.VincularNFe;

public sealed class VincularNFeCommandHandler
    : IRequestHandler<VincularNFeCommand, ResponseDefault<VincularNFeCommandResult>>
{
    private readonly IRecebimentoCompraRepository _repo;

    public VincularNFeCommandHandler(IRecebimentoCompraRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<VincularNFeCommandResult>> Handle(VincularNFeCommand request, CancellationToken cancellationToken)
    {
        var recebimento = await _repo.GetByIdAsync(request.RecebimentoId, cancellationToken);
        if (recebimento is null)
            return ResponseDefault<VincularNFeCommandResult>.NotFound("Recebimento não encontrado.");

        var chave = NFeChaveAcessoHelper.OnlyDigits(request.ChaveAcesso);
        var valida = NFeChaveAcessoHelper.IsValid(chave);
        if (!valida)
            return ResponseDefault<VincularNFeCommandResult>.BadRequest(
                Core.Response.Erros.Error.Validation("Chave de acesso inválida."));

        // Validação na SEFAZ é stub: a integração real exige certificado A1, webservice
        // por estado e parsing do XML retornado. A migração `fiscal-nfe` cobrirá isso.
        const bool consultaSefazExecutada = false;

        await _repo.VincularNFeAsync(recebimento.Id, request.NumeroNotaFiscal, chave, cancellationToken);

        return ResponseDefault<VincularNFeCommandResult>.Ok(
            new VincularNFeCommandResult(recebimento.Id, chave, valida, consultaSefazExecutada));
    }
}
