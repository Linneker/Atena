using Acme.Sistemas.Services.V1.Rh.Funcionario.Command.CriarFuncionarioCompleto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.CriarFuncionarioCompleto;

public static class CriarFuncionarioCompletoMap
{
    public static CriarFuncionarioCompletoCommand ToCommand(this CriarFuncionarioCompletoRequest r)
        => new(
            r.NomeCompleto, r.Cpf, r.Email, r.Telefone, r.DataNascimento,
            r.EstadoCivil, r.Naturalidade, r.Nacionalidade,
            r.Rg, r.RgOrgao, r.RgUf, r.Endereco,
            r.DataAdmissao, r.CargoId, r.LotacaoId, r.DepartamentoId, r.CentroDeCustoId,
            r.TipoContrato, r.RegimeRemuneracao, r.CodigoMatricula,
            r.Pis, r.Ctps, r.CtpsSerie, r.CtpsUf,
            r.SalarioInicial, r.ContaBancaria, r.JornadaId,
            r.Beneficios?.Select(b => new BeneficioInicialDto(
                b.BeneficioCatalogoId, b.Valor, b.DescontoFuncionarioPct, b.VigenciaInicio)).ToList(),
            r.Dependentes?.Select(d => new DependenteInicialDto(
                d.NomeCompleto, d.Cpf, d.DataNascimento, d.Tipo,
                d.Irrf, d.SalarioFamilia, d.PensaoAlimenticiaPct)).ToList());

    public static CriarFuncionarioCompletoResponse ToResponse(this CriarFuncionarioCompletoCommandResult r)
        => new(r.FuncionarioId, r.HistoricoSalarioId, r.EscalaId, r.BeneficiosCriados, r.DependentesCriados);
}
