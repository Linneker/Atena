using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using FuncionarioEntity = Acme.Sistemas.Domain.Entities.Cadastros.Funcionario;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.CriarFuncionarioCompleto;

public sealed class CriarFuncionarioCompletoCommandHandler
    : IRequestHandler<CriarFuncionarioCompletoCommand, ResponseDefault<CriarFuncionarioCompletoCommandResult>>
{
    private readonly IFuncionarioRepository _funcRepo;
    private readonly IHistoricoSalarioRepository _histRepo;
    private readonly IEscalaFuncionarioRepository _escalaRepo;
    private readonly IBeneficioFuncionarioRepository _benefRepo;
    private readonly IDependenteRepository _depRepo;
    private readonly ITenantContext _tenantContext;

    public CriarFuncionarioCompletoCommandHandler(
        IFuncionarioRepository funcRepo,
        IHistoricoSalarioRepository histRepo,
        IEscalaFuncionarioRepository escalaRepo,
        IBeneficioFuncionarioRepository benefRepo,
        IDependenteRepository depRepo,
        ITenantContext tenantContext)
    {
        _funcRepo = funcRepo;
        _histRepo = histRepo;
        _escalaRepo = escalaRepo;
        _benefRepo = benefRepo;
        _depRepo = depRepo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarFuncionarioCompletoCommandResult>> Handle(
        CriarFuncionarioCompletoCommand request, CancellationToken cancellationToken)
    {
        // Idempotência por CPF (single tenant).
        var existeCpf = await _funcRepo.GetByCpfAsync(request.Cpf, cancellationToken);
        if (existeCpf is not null)
            return ResponseDefault<CriarFuncionarioCompletoCommandResult>.Conflict(
                $"Já existe funcionário com CPF '{request.Cpf}'.");

        if (!string.IsNullOrWhiteSpace(request.CodigoMatricula))
        {
            var existeMat = await _funcRepo.GetByMatriculaAsync(request.CodigoMatricula, cancellationToken);
            if (existeMat is not null)
                return ResponseDefault<CriarFuncionarioCompletoCommandResult>.Conflict(
                    $"Matrícula '{request.CodigoMatricula}' já está em uso.");
        }

        var userId = _tenantContext.UserId;
        var tenantId = _tenantContext.TenantId;

        var func = new FuncionarioEntity
        {
            TenantId = tenantId,
            NomeCompleto = request.NomeCompleto,
            Cpf = request.Cpf,
            Email = request.Email,
            Telefone = request.Telefone,
            DataAdmissao = request.DataAdmissao.ToDateTime(TimeOnly.MinValue),
            Status = StatusAtivo.Ativo,
            CargoId = request.CargoId,
            LotacaoId = request.LotacaoId,
            DepartamentoId = request.DepartamentoId,
            CentroDeCustoId = request.CentroDeCustoId,
            TipoContrato = request.TipoContrato,
            RegimeRemuneracao = request.RegimeRemuneracao,
            CodigoMatricula = request.CodigoMatricula,
            Pis = request.Pis,
            Ctps = request.Ctps,
            CtpsSerie = request.CtpsSerie,
            CtpsUf = request.CtpsUf,
            Rg = request.Rg,
            RgOrgao = request.RgOrgao,
            RgUf = request.RgUf,
            EstadoCivil = request.EstadoCivil,
            Naturalidade = request.Naturalidade,
            Nacionalidade = request.Nacionalidade ?? "Brasileira",
            Endereco = request.Endereco,
            ContaBancaria = request.ContaBancaria,
            CreatedBy = userId,
        };
        await _funcRepo.AddAsync(func, cancellationToken);

        var hist = new HistoricoSalario
        {
            TenantId = tenantId,
            FuncionarioId = func.Id,
            Valor = request.SalarioInicial,
            VigenciaInicio = request.DataAdmissao,
            Motivo = MotivoSalario.Admissao,
            RegistradoPorUsuarioId = userId,
            RegistradoAt = DateTime.UtcNow,
            CreatedBy = userId,
        };
        await _histRepo.AddAsync(hist, cancellationToken);

        Guid? escalaId = null;
        if (request.JornadaId.HasValue)
        {
            var escala = new EscalaFuncionario
            {
                TenantId = tenantId,
                FuncionarioId = func.Id,
                JornadaId = request.JornadaId.Value,
                VigenciaInicio = request.DataAdmissao,
                CreatedBy = userId,
            };
            await _escalaRepo.AddAsync(escala, cancellationToken);
            escalaId = escala.Id;
        }

        var beneficiosCriados = 0;
        if (request.Beneficios is not null)
        {
            foreach (var b in request.Beneficios)
            {
                var bf = new BeneficioFuncionario
                {
                    TenantId = tenantId,
                    FuncionarioId = func.Id,
                    BeneficioCatalogoId = b.BeneficioCatalogoId,
                    Valor = b.Valor,
                    DescontoFuncionarioPct = b.DescontoFuncionarioPct,
                    VigenciaInicio = b.VigenciaInicio,
                    CreatedBy = userId,
                };
                await _benefRepo.AddAsync(bf, cancellationToken);
                beneficiosCriados++;
            }
        }

        var dependentesCriados = 0;
        if (request.Dependentes is not null)
        {
            foreach (var d in request.Dependentes)
            {
                var dep = new Dependente
                {
                    TenantId = tenantId,
                    FuncionarioId = func.Id,
                    NomeCompleto = d.NomeCompleto,
                    Cpf = d.Cpf,
                    DataNascimento = d.DataNascimento,
                    Tipo = d.Tipo,
                    Irrf = d.Irrf,
                    SalarioFamilia = d.SalarioFamilia,
                    PensaoAlimenticiaPct = d.PensaoAlimenticiaPct,
                    CreatedBy = userId,
                };
                await _depRepo.AddAsync(dep, cancellationToken);
                dependentesCriados++;
            }
        }

        return ResponseDefault<CriarFuncionarioCompletoCommandResult>.Created(
            new CriarFuncionarioCompletoCommandResult(
                func.Id, hist.Id, escalaId, beneficiosCriados, dependentesCriados));
    }
}
