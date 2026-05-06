using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.ConciliacaoBancaria.Parsers;
using Acme.Sistemas.Services.V1.ConciliacaoBancaria.Services;

namespace Acme.Sistemas.Services.V1.ConciliacaoBancaria.Command.ImportarExtrato;

public sealed class ImportarExtratoCommandHandler
    : IRequestHandler<ImportarExtratoCommand, ResponseDefault<ImportarExtratoCommandResult>>
{
    private readonly IConciliacaoBancariaRepository _repo;
    private readonly ConciliacaoMatcher _matcher;
    private readonly ITenantContext _tenantContext;

    public ImportarExtratoCommandHandler(
        IConciliacaoBancariaRepository repo,
        ConciliacaoMatcher matcher,
        ITenantContext tenantContext)
    {
        _repo = repo;
        _matcher = matcher;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<ImportarExtratoCommandResult>> Handle(
        ImportarExtratoCommand request, CancellationToken cancellationToken)
    {
        IExtratoParser parser = request.Formato.Equals("OFX", StringComparison.OrdinalIgnoreCase)
            ? new OfxExtratoParser()
            : new CsvExtratoParser();

        IReadOnlyList<ParsedExtratoItem> parsed;
        using (var ms = new MemoryStream(request.Conteudo))
        {
            parsed = parser.Parse(ms);
        }

        if (parsed.Count == 0)
        {
            return ResponseDefault<ImportarExtratoCommandResult>.BadRequest(
                Core.Response.Erros.Error.Validation("Nenhum lançamento válido encontrado no extrato."));
        }

        var conciliacao = new Domain.Entities.Financeiro.ConciliacaoBancaria
        {
            TenantId = _tenantContext.TenantId,
            Banco = request.Banco,
            Agencia = request.Agencia,
            Conta = request.Conta,
            PeriodoInicio = parsed.Min(x => x.DataMovimento),
            PeriodoFim = parsed.Max(x => x.DataMovimento),
            FormatoArquivo = parser.Formato,
            Status = StatusConciliacao.Importado,
            TotalLancamentos = parsed.Count,
            CreatedBy = _tenantContext.UserId
        };

        await _repo.AddAsync(conciliacao, cancellationToken);

        var itens = parsed.Select(p => new ItemExtrato
        {
            TenantId = _tenantContext.TenantId,
            ConciliacaoId = conciliacao.Id,
            DataMovimento = p.DataMovimento,
            Valor = p.Valor,
            Tipo = p.Tipo,
            Descricao = p.Descricao,
            DocumentoBancario = p.DocumentoBancario,
            Status = StatusItemExtrato.NaoConciliado,
            CreatedBy = _tenantContext.UserId
        }).ToList();

        var totalConciliados = await _matcher.ConciliarAsync(itens, diasTolerancia: 3, cancellationToken);

        await _repo.AddItensAsync(itens, cancellationToken);
        await _repo.UpdateTotaisAsync(
            conciliacao.Id,
            itens.Count,
            totalConciliados,
            totalConciliados == itens.Count ? StatusConciliacao.Concluido : StatusConciliacao.EmRevisao,
            cancellationToken);

        return ResponseDefault<ImportarExtratoCommandResult>.Created(
            new ImportarExtratoCommandResult(conciliacao.Id, itens.Count, totalConciliados));
    }
}
