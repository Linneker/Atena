using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Query.ObterFichaCompleta;

public sealed class ObterFichaCompletaQueryHandler
    : IRequestHandler<ObterFichaCompletaQuery, ResponseDefault<ObterFichaCompletaQueryResult>>
{
    private readonly IFuncionarioRepository _funcRepo;
    private readonly IHistoricoSalarioRepository _histRepo;
    private readonly IBeneficioFuncionarioRepository _benefRepo;
    private readonly IDependenteRepository _depRepo;
    private readonly IEscalaFuncionarioRepository _escalaRepo;

    public ObterFichaCompletaQueryHandler(
        IFuncionarioRepository funcRepo,
        IHistoricoSalarioRepository histRepo,
        IBeneficioFuncionarioRepository benefRepo,
        IDependenteRepository depRepo,
        IEscalaFuncionarioRepository escalaRepo)
    {
        _funcRepo = funcRepo;
        _histRepo = histRepo;
        _benefRepo = benefRepo;
        _depRepo = depRepo;
        _escalaRepo = escalaRepo;
    }

    public async Task<ResponseDefault<ObterFichaCompletaQueryResult>> Handle(
        ObterFichaCompletaQuery request, CancellationToken cancellationToken)
    {
        var f = await _funcRepo.GetByIdAsync(request.FuncionarioId, cancellationToken);
        if (f is null)
            return ResponseDefault<ObterFichaCompletaQueryResult>.NotFound(
                $"Funcionário {request.FuncionarioId} não encontrado.");

        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);

        var hist = await _histRepo.ListByFuncionarioAsync(f.Id, cancellationToken);
        var vigente = await _histRepo.GetVigenteAsync(f.Id, hoje, cancellationToken);
        var benef = await _benefRepo.ListByFuncionarioAsync(f.Id, cancellationToken);
        var deps = await _depRepo.ListByFuncionarioAsync(f.Id, cancellationToken);
        var escalas = await _escalaRepo.ListByFuncionarioAsync(f.Id, cancellationToken);

        var ficha = new ObterFichaCompletaQueryResult(
            DadosPessoais: new FichaDadosPessoais(
                f.Id, f.NomeCompleto, f.Cpf, f.Email, f.Telefone,
                f.Rg, f.RgOrgao, f.RgUf, f.EstadoCivil, f.Naturalidade, f.Nacionalidade,
                f.Endereco, f.ContaBancaria),
            Contrato: new FichaContrato(
                f.DataAdmissao, f.DataDemissao, f.CargoId, f.LotacaoId, f.DepartamentoId,
                f.CentroDeCustoId, f.TipoContrato, f.RegimeRemuneracao, f.CodigoMatricula,
                f.Pis, f.Ctps, f.CtpsSerie, f.CtpsUf, f.Status),
            SalarioVigente: vigente?.Valor,
            HistoricoSalarial: hist.Select(h => new FichaSalarioItem(
                h.Id, h.Valor, h.VigenciaInicio, h.VigenciaFim, h.Motivo, h.Observacao)).ToList(),
            Beneficios: benef.Select(b => new FichaBeneficioItem(
                b.Id, b.BeneficioCatalogoId, b.Valor, b.DescontoFuncionarioPct,
                b.VigenciaInicio, b.VigenciaFim)).ToList(),
            Dependentes: deps.Select(d => new FichaDependenteItem(
                d.Id, d.NomeCompleto, d.Cpf, d.DataNascimento, d.Tipo,
                d.Irrf, d.SalarioFamilia, d.PensaoAlimenticiaPct)).ToList(),
            Escalas: escalas.Select(e => new FichaEscalaItem(
                e.Id, e.JornadaId, e.VigenciaInicio, e.VigenciaFim, e.Observacao)).ToList());

        return ResponseDefault<ObterFichaCompletaQueryResult>.Ok(ficha);
    }
}
