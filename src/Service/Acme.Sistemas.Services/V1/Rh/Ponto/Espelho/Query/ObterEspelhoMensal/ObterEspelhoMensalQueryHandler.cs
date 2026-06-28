using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Services.V1.Rh.Ponto.Engine;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Espelho.Query.ObterEspelhoMensal;

public sealed class ObterEspelhoMensalQueryHandler
    : IRequestHandler<ObterEspelhoMensalQuery, ResponseDefault<GeradorEspelhoMensal.EspelhoMensal>>
{
    private readonly IFuncionarioRepository _funcRepo;
    private readonly IJornadaRepository _jornadaRepo;
    private readonly IMarcacaoPontoRepository _marcRepo;
    private readonly IEscalaFuncionarioRepository _escalaRepo;

    public ObterEspelhoMensalQueryHandler(
        IFuncionarioRepository funcRepo,
        IJornadaRepository jornadaRepo,
        IMarcacaoPontoRepository marcRepo,
        IEscalaFuncionarioRepository escalaRepo)
    {
        _funcRepo = funcRepo;
        _jornadaRepo = jornadaRepo;
        _marcRepo = marcRepo;
        _escalaRepo = escalaRepo;
    }

    public async Task<ResponseDefault<GeradorEspelhoMensal.EspelhoMensal>> Handle(
        ObterEspelhoMensalQuery request, CancellationToken cancellationToken)
    {
        var func = await _funcRepo.GetByIdAsync(request.FuncionarioId, cancellationToken);
        if (func is null)
            return ResponseDefault<GeradorEspelhoMensal.EspelhoMensal>.NotFound(
                $"Funcionário {request.FuncionarioId} não encontrado.");

        if (!DateOnly.TryParseExact(request.Competencia + "-01", "yyyy-MM-dd", out var primeiroDia))
            return ResponseDefault<GeradorEspelhoMensal.EspelhoMensal>.BadRequest(
                Error.Validation($"competência '{request.Competencia}' inválida; esperado YYYY-MM."));

        var ultimoDia = primeiroDia.AddMonths(1).AddDays(-1);

        // Localiza jornada vigente em algum dia do mês — heurística: usa a vigente no primeiro dia.
        var escala = await _escalaRepo.GetVigenteAsync(func.Id, primeiroDia, cancellationToken);
        var jornada = escala is null
            ? null
            : await _jornadaRepo.GetByIdAsync(escala.JornadaId, cancellationToken);

        if (jornada is null)
            return ResponseDefault<GeradorEspelhoMensal.EspelhoMensal>.NotFound(
                $"Funcionário {request.FuncionarioId} sem jornada vigente em {request.Competencia}.");

        var marcacoes = await _marcRepo.ListByFuncionarioPeriodoAsync(func.Id, primeiroDia, ultimoDia, cancellationToken);

        var espelho = GeradorEspelhoMensal.Gerar(
            func, request.Competencia, jornada,
            politica: null,
            marcacoes: marcacoes,
            feriados: Array.Empty<Domain.Entities.Rh.Feriado>());

        return ResponseDefault<GeradorEspelhoMensal.EspelhoMensal>.Ok(espelho);
    }
}
