using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Receita.Command.ExcluirReceita;

public sealed class ExcluirReceitaCommandHandler : IRequestHandler<ExcluirReceitaCommand, ResponseDefault>
{
    private readonly IReceitaRepository _receitas;

    public ExcluirReceitaCommandHandler(IReceitaRepository receitas)
    {
        _receitas = receitas;
    }

    public async Task<ResponseDefault> Handle(ExcluirReceitaCommand request, CancellationToken cancellationToken)
    {
        var receita = await _receitas.GetByIdAsync(request.Id, cancellationToken);
        if (receita is null)
            return ResponseDefault.BadRequest(Error.NotFound("Receita não encontrada."));

        if (receita.StatusRecebimento == StatusPagamento.Pago)
            return ResponseDefault.BadRequest(Error.Conflict(
                "Não é possível excluir uma receita já recebida. Cancele o lançamento de recebimento primeiro."));

        await _receitas.DeleteAsync(request.Id, cancellationToken);

        return ResponseDefault.NoContent();
    }
}
