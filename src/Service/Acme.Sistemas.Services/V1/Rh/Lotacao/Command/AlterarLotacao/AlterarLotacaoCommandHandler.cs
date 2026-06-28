using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Command.AlterarLotacao;

public sealed class AlterarLotacaoCommandHandler
    : IRequestHandler<AlterarLotacaoCommand, ResponseDefault<AlterarLotacaoCommandResult>>
{
    private readonly ILotacaoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AlterarLotacaoCommandHandler(ILotacaoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarLotacaoCommandResult>> Handle(
        AlterarLotacaoCommand request, CancellationToken cancellationToken)
    {
        var lotacao = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (lotacao is null)
            return ResponseDefault<AlterarLotacaoCommandResult>.NotFound(
                $"Lotação {request.Id} não encontrada.");

        if (!string.Equals(lotacao.Nome, request.Nome, StringComparison.OrdinalIgnoreCase))
        {
            var conflito = await _repo.GetByNomeAsync(request.Nome, cancellationToken);
            if (conflito is not null && conflito.Id != request.Id)
                return ResponseDefault<AlterarLotacaoCommandResult>.Conflict(
                    $"Já existe uma lotação com o nome '{request.Nome}'.");
        }

        lotacao.Nome = request.Nome;
        lotacao.EmpresaId = request.EmpresaId;
        lotacao.Cnpj = request.Cnpj;
        lotacao.EnderecoJson = request.EnderecoJson;
        lotacao.Ativo = request.Ativo;
        lotacao.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(lotacao, cancellationToken);

        return ResponseDefault<AlterarLotacaoCommandResult>.Ok(
            new AlterarLotacaoCommandResult(lotacao.Id));
    }
}
