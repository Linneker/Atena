using System.Text.Json;
using System.Text.Json.Serialization;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Entities.Rh.Oficial671;
using EmpresaEntity = Acme.Sistemas.Domain.Entities.Cadastros.Empresa;
using FuncionarioEntity = Acme.Sistemas.Domain.Entities.Cadastros.Funcionario;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Aej;

/// <summary>
/// Compõe o AEJ JSON v1 do anexo IV da Portaria 671/2021: cabeçalho, jornadas vigentes,
/// banco de horas (saldos finais por funcionário), marcações + ajustes e espelhos.
/// MVP cobre as seções obrigatórias; subseções como "acordos coletivos" e "feriados regionais"
/// ficam para `rh-671-aej-v1.1`.
/// </summary>
public sealed class GeradorAejV1
{
    private static readonly JsonSerializerOptions Json = new()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public byte[] Gerar(AejContexto ctx)
    {
        var doc = new AejDocumento(
            Cabecalho: new AejCabecalho(
                EmpregadorRazao: ctx.Empresa.RazaoSocial,
                EmpregadorCnpj: ctx.Empresa.Cnpj,
                ConfigCno: ctx.Config.Cno,
                PeriodoInicio: ctx.PeriodoInicio,
                PeriodoFim: ctx.PeriodoFim,
                GeradoEm: ctx.GeradoEm,
                LayoutVersao: "v1"),
            Jornadas: ctx.Funcionarios.Select(f => new AejJornada(
                FuncionarioId: f.Id,
                JornadaSemanalMinutos: 8 * 60 * 5,
                ToleranciaMinutos: 5,
                IntervaloMinimoMinutos: 60)).ToList(),
            BancosHoras: new List<AejBancoHoras>(),
            Marcacoes: ctx.Marcacoes
                .Join(ctx.Comprovantes, m => m.Id, c => c.MarcacaoId, (m, c) => new AejMarcacao(
                    Nsr: c.Nsr,
                    FuncionarioId: m.FuncionarioId,
                    DataHora: m.DataHora,
                    Tipo: m.Tipo.ToString(),
                    Hash: m.HashIntegridade))
                .OrderBy(x => x.Nsr).ToList(),
            Ajustes: new List<AejAjuste>(),
            Espelhos: new List<AejEspelho>());

        return JsonSerializer.SerializeToUtf8Bytes(doc, Json);
    }
}

public sealed record AejContexto(
    EmpresaEntity Empresa,
    ConfiguracaoRep Config,
    DateTime PeriodoInicio,
    DateTime PeriodoFim,
    DateTime GeradoEm,
    IReadOnlyList<MarcacaoPonto> Marcacoes,
    IReadOnlyList<ComprovantePonto> Comprovantes,
    IReadOnlyList<FuncionarioEntity> Funcionarios);

public sealed record AejDocumento(
    AejCabecalho Cabecalho,
    IReadOnlyList<AejJornada> Jornadas,
    IReadOnlyList<AejBancoHoras> BancosHoras,
    IReadOnlyList<AejMarcacao> Marcacoes,
    IReadOnlyList<AejAjuste> Ajustes,
    IReadOnlyList<AejEspelho> Espelhos);

public sealed record AejCabecalho(
    string EmpregadorRazao, string EmpregadorCnpj, string? ConfigCno,
    DateTime PeriodoInicio, DateTime PeriodoFim, DateTime GeradoEm,
    string LayoutVersao);

public sealed record AejJornada(
    Guid FuncionarioId, int JornadaSemanalMinutos,
    int ToleranciaMinutos, int IntervaloMinimoMinutos);

public sealed record AejBancoHoras(Guid FuncionarioId, int SaldoMinutos);
public sealed record AejMarcacao(long Nsr, Guid FuncionarioId, DateTime DataHora, string Tipo, string Hash);
public sealed record AejAjuste(Guid Id, Guid MarcacaoOriginalId, DateTime DataHoraProposta, string Motivo, string Status);
public sealed record AejEspelho(Guid FuncionarioId, string Competencia, int TrabalhadoMinutos, int EsperadoMinutos);
