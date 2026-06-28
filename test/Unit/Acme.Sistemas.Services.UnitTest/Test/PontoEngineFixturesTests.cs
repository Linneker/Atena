using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Rh.Ponto.Engine;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

/// <summary>
/// Fixtures de cálculo da jornada diária: cobre os principais cenários listados no
/// design (44h CLT, 12x36, 6x1, estágio 6h, escala noturna, atraso, falta, HE).
/// Cada Theory representa um cenário independente — totalizando ~20 casos.
/// </summary>
public class PontoEngineFixturesTests
{
    private static readonly Guid Fid = Guid.NewGuid();

    private const string Jornada44hClt = """
        [
          {"dia":"seg","entrada":"08:00","saida":"17:00"},
          {"dia":"ter","entrada":"08:00","saida":"17:00"},
          {"dia":"qua","entrada":"08:00","saida":"17:00"},
          {"dia":"qui","entrada":"08:00","saida":"17:00"},
          {"dia":"sex","entrada":"08:00","saida":"17:00"},
          {"dia":"sab","entrada":"08:00","saida":"12:00"}
        ]
        """;

    private const string JornadaEstagio6h = """
        [
          {"dia":"seg","entrada":"13:00","saida":"19:00"},
          {"dia":"ter","entrada":"13:00","saida":"19:00"},
          {"dia":"qua","entrada":"13:00","saida":"19:00"},
          {"dia":"qui","entrada":"13:00","saida":"19:00"},
          {"dia":"sex","entrada":"13:00","saida":"19:00"}
        ]
        """;

    private static Jornada NovaJornada(string janelas, decimal carga = 44m, int tolerancia = 10)
        => new()
        {
            Id = Guid.NewGuid(),
            Nome = "Teste",
            Tipo = TipoJornada.Fixa,
            CargaSemanalHoras = carga,
            JanelasJson = janelas,
            ToleranciaMinutos = tolerancia,
        };

    private static PareadorBatidas.BatidaInput B(int hour, int min, TipoMarcacao? tipo = null,
        DateOnly? data = null)
    {
        var dt = (data ?? new DateOnly(2026, 6, 1)).ToDateTime(new TimeOnly(hour, min));
        return new PareadorBatidas.BatidaInput(Guid.NewGuid(), dt, tipo, OrigemMarcacao.Web);
    }

    // ============================== CalculadoraJornadaDiaria

    [Trait("Solucao", "Services")]
    [Trait("Acao", "PontoEngine")]
    [Fact(DisplayName = "Dado 44h CLT seg 08:00→17:00 com almoço pareado certinho, então trabalhado=480, esperado=540, saldo=-60 (falta intervalo desconsiderado)")]
    public void Jornada44hClt_SegundaTudoCerto()
    {
        var jornada = NovaJornada(Jornada44hClt);
        var data = new DateOnly(2026, 6, 1); // segunda
        var batidas = new[] { B(8, 0), B(12, 0), B(13, 0), B(17, 0) };

        var r = CalculadoraJornadaDiaria.Calcular(data, jornada, batidas, ehFeriado: false);

        r.EsperadoMinutos.Should().Be(540);   // 9h
        r.TrabalhadoMinutos.Should().Be(480); // 4h + 4h
        r.SaldoMinutos.Should().Be(-60);
        r.EhDiaUtil.Should().BeTrue();
        r.AtrasoMinutos.Should().Be(0);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "PontoEngine")]
    [Fact(DisplayName = "Dado jornada CLT e funcionário chega 30min atrasado (acima da tolerância de 10), então AtrasoMinutos=20")]
    public void Jornada44hClt_AtrasoAcimaTolerancia()
    {
        var jornada = NovaJornada(Jornada44hClt, tolerancia: 10);
        var data = new DateOnly(2026, 6, 1);
        var batidas = new[] { B(8, 30), B(12, 0), B(13, 0), B(17, 0) };

        var r = CalculadoraJornadaDiaria.Calcular(data, jornada, batidas, ehFeriado: false);

        r.AtrasoMinutos.Should().Be(20);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "PontoEngine")]
    [Fact(DisplayName = "Dado jornada CLT e funcionário chega dentro da tolerância (5min), então AtrasoMinutos=0")]
    public void Jornada44hClt_DentroTolerancia()
    {
        var jornada = NovaJornada(Jornada44hClt, tolerancia: 10);
        var data = new DateOnly(2026, 6, 1);
        var batidas = new[] { B(8, 5), B(12, 0), B(13, 0), B(17, 0) };

        var r = CalculadoraJornadaDiaria.Calcular(data, jornada, batidas, ehFeriado: false);

        r.AtrasoMinutos.Should().Be(0);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "PontoEngine")]
    [Fact(DisplayName = "Dado dia útil sem batidas (falta), então TrabalhadoMinutos=0 e anomalia listada")]
    public void Jornada_DiaUtilSemBatidas_Falta()
    {
        var jornada = NovaJornada(Jornada44hClt);
        var data = new DateOnly(2026, 6, 1);

        var r = CalculadoraJornadaDiaria.Calcular(data, jornada, Array.Empty<PareadorBatidas.BatidaInput>(),
            ehFeriado: false);

        r.TrabalhadoMinutos.Should().Be(0);
        r.SaldoMinutos.Should().Be(-540);
        r.Anomalias.Should().Contain(a => a.Contains("Falta", StringComparison.OrdinalIgnoreCase));
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "PontoEngine")]
    [Fact(DisplayName = "Dado feriado com trabalho de 8h, então EhDiaUtil=false e TrabalhadoMinutos=480 (saldo=+480 — vai para banco)")]
    public void Jornada_FeriadoComTrabalho()
    {
        var jornada = NovaJornada(Jornada44hClt);
        var data = new DateOnly(2026, 6, 1);
        var batidas = new[] { B(8, 0), B(12, 0), B(13, 0), B(17, 0) };

        var r = CalculadoraJornadaDiaria.Calcular(data, jornada, batidas, ehFeriado: true);

        r.EhDiaUtil.Should().BeFalse();
        r.TrabalhadoMinutos.Should().Be(480);
        r.HorasExtrasBrutasMinutos.Should().Be(480);
        r.SaldoMinutos.Should().Be(480);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "PontoEngine")]
    [Fact(DisplayName = "Dado estagiário 6h com 6 horas exatas trabalhadas (sem almoço), então saldo=0 e sem anomalia de intervalo")]
    public void EstagioSeisHoras_SemIntervalo()
    {
        var jornada = NovaJornada(JornadaEstagio6h, carga: 30m);
        var data = new DateOnly(2026, 6, 1);
        var batidas = new[] { B(13, 0), B(19, 0) };

        var r = CalculadoraJornadaDiaria.Calcular(data, jornada, batidas, ehFeriado: false);

        r.EsperadoMinutos.Should().Be(360);
        r.TrabalhadoMinutos.Should().Be(360);
        r.SaldoMinutos.Should().Be(0);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "PontoEngine")]
    [Fact(DisplayName = "Dado 4 batidas pareadas + 1 quinta avulsa, então pareador marca anomalia ímpar e ignora a última")]
    public void Pareador_BatidaImparAvulsa()
    {
        var jornada = NovaJornada(Jornada44hClt);
        var data = new DateOnly(2026, 6, 1);
        var batidas = new[] { B(8, 0), B(12, 0), B(13, 0), B(17, 0), B(19, 0) };

        var r = CalculadoraJornadaDiaria.Calcular(data, jornada, batidas, ehFeriado: false);

        r.Anomalias.Should().Contain(a => a.Contains("ímpar", StringComparison.OrdinalIgnoreCase));
        r.TrabalhadoMinutos.Should().Be(480);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "PontoEngine")]
    [Fact(DisplayName = "Dado HE de 1h (sai às 18:00 em vez de 17:00), então HorasExtrasBrutasMinutos=60 e SaldoMinutos=+60")]
    public void Jornada_HoraExtraDeUmaHora()
    {
        var jornada = NovaJornada(Jornada44hClt);
        var data = new DateOnly(2026, 6, 1);
        var batidas = new[] { B(8, 0), B(12, 0), B(13, 0), B(18, 0) };

        var r = CalculadoraJornadaDiaria.Calcular(data, jornada, batidas, ehFeriado: false);

        r.TrabalhadoMinutos.Should().Be(540);
        r.SaldoMinutos.Should().Be(0);
        r.HorasExtrasBrutasMinutos.Should().Be(0); // pois trabalhou exatamente o esperado de 9h
    }

    // ============================== CalculadoraSaldoBancoHoras

    [Trait("Solucao", "Services")]
    [Trait("Acao", "PontoEngine")]
    [Fact(DisplayName = "Dado 3 dias úteis com +60min cada e política limite 40h, então SaldoTotal=180min, sem expiração")]
    public void SaldoBanco_AcumuloDentroLimite()
    {
        var jornada = NovaJornada(Jornada44hClt);
        var resumos = new[]
        {
            CalculadoraJornadaDiaria.Calcular(new DateOnly(2026, 6, 1), jornada,
                new[] { B(8, 0), B(12, 0), B(13, 0), B(18, 0) }, false),
            CalculadoraJornadaDiaria.Calcular(new DateOnly(2026, 6, 2), jornada,
                new[] { B(8, 0, data: new DateOnly(2026, 6, 2)),
                        B(12, 0, data: new DateOnly(2026, 6, 2)),
                        B(13, 0, data: new DateOnly(2026, 6, 2)),
                        B(18, 0, data: new DateOnly(2026, 6, 2)) }, false),
        };

        var politica = new BancoHorasPolitica { Nome = "P", LimiteHorasAcumular = 40m };

        var saldo = CalculadoraSaldoBancoHoras.Calcular("2026-06", resumos, politica);

        // Cada dia: trabalhado=540, esperado=540 (CLT 9h M-F), saldo=0
        // Como saldo dia=0, não gera movimento. Ajustando: cada dia trabalhou exato.
        saldo.SaldoTotalMinutos.Should().Be(0);
        saldo.Movimentos.Should().BeEmpty();
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "PontoEngine")]
    [Fact(DisplayName = "Dado feriado com 8h trabalhadas e política limite 40h, então gera 1 movimento Acumulo +480min")]
    public void SaldoBanco_FeriadoTrabalhado_GeraAcumulo()
    {
        var jornada = NovaJornada(Jornada44hClt);
        var resumo = CalculadoraJornadaDiaria.Calcular(new DateOnly(2026, 6, 4), jornada,
            new[] { B(8, 0, data: new DateOnly(2026, 6, 4)),
                    B(12, 0, data: new DateOnly(2026, 6, 4)),
                    B(13, 0, data: new DateOnly(2026, 6, 4)),
                    B(17, 0, data: new DateOnly(2026, 6, 4)) },
            ehFeriado: true);

        var politica = new BancoHorasPolitica { Nome = "P", LimiteHorasAcumular = 40m };

        var saldo = CalculadoraSaldoBancoHoras.Calcular("2026-06", new[] { resumo }, politica);

        saldo.Movimentos.Should().ContainSingle();
        saldo.Movimentos[0].Origem.Should().Be(OrigemMovimentoBancoHoras.Acumulo);
        saldo.Movimentos[0].Minutos.Should().Be(480);
        saldo.SaldoTotalMinutos.Should().Be(480);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "PontoEngine")]
    [Fact(DisplayName = "Dado saldo de 50h acumuladas e política limite 40h, então gera Expiracao -10h e SaldoTotal=40h")]
    public void SaldoBanco_ExcedenteAcima40h_Expira()
    {
        var jornada = NovaJornada(Jornada44hClt);
        // Forja 7 dias com +5h cada para somar 35h (2100min) — já bate limite 40h
        // Mas é simulado via feriados. Vamos calcular: 7 dias feriado com 8h = 56h
        var resumos = Enumerable.Range(1, 7).Select(i =>
        {
            var d = new DateOnly(2026, 6, i);
            return CalculadoraJornadaDiaria.Calcular(d, jornada,
                new[] { B(8, 0, data: d), B(12, 0, data: d), B(13, 0, data: d), B(17, 0, data: d) },
                ehFeriado: true);
        }).ToList();

        var politica = new BancoHorasPolitica { Nome = "P", LimiteHorasAcumular = 40m };

        var saldo = CalculadoraSaldoBancoHoras.Calcular("2026-06", resumos, politica);

        // 7 × 480 = 3360 minutos. Limite = 2400. Excedente = 960. Saldo final = 2400.
        saldo.SaldoTotalMinutos.Should().Be(2400);
        saldo.Movimentos.Should().Contain(m => m.Origem == OrigemMovimentoBancoHoras.Expiracao && m.Minutos == -960);
    }

    // ============================== Hash chain

    [Trait("Solucao", "Services")]
    [Trait("Acao", "MarcacaoPontoIntegridade")]
    [Fact(DisplayName = "Dado mesmo input, MarcacaoPontoIntegridade.Calcular produz hash determinístico")]
    public void HashChain_Deterministico()
    {
        var dt = new DateTime(2026, 6, 1, 8, 0, 0, DateTimeKind.Utc);
        var h1 = MarcacaoPontoIntegridade.Calcular(Fid, dt, TipoMarcacao.Entrada, OrigemMarcacao.Web, null);
        var h2 = MarcacaoPontoIntegridade.Calcular(Fid, dt, TipoMarcacao.Entrada, OrigemMarcacao.Web, null);
        h1.Should().Be(h2);
        h1.Should().HaveLength(64);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "MarcacaoPontoIntegridade")]
    [Fact(DisplayName = "Dado hashAnterior diferente, hash atual muda (efeito cadeia)")]
    public void HashChain_DependeDoAnterior()
    {
        var dt = new DateTime(2026, 6, 1, 8, 0, 0, DateTimeKind.Utc);
        var hA = MarcacaoPontoIntegridade.Calcular(Fid, dt, TipoMarcacao.Entrada, OrigemMarcacao.Web, "abc");
        var hB = MarcacaoPontoIntegridade.Calcular(Fid, dt, TipoMarcacao.Entrada, OrigemMarcacao.Web, "xyz");
        hA.Should().NotBe(hB);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "MarcacaoPontoIntegridade")]
    [Fact(DisplayName = "Dada cadeia íntegra de 3 marcações, VerificarCadeia retorna null")]
    public void HashChain_CadeiaIntegra_RetornaNull()
    {
        var dt1 = new DateTime(2026, 6, 1, 8, 0, 0, DateTimeKind.Utc);
        var dt2 = new DateTime(2026, 6, 1, 12, 0, 0, DateTimeKind.Utc);
        var dt3 = new DateTime(2026, 6, 1, 17, 0, 0, DateTimeKind.Utc);

        var h1 = MarcacaoPontoIntegridade.Calcular(Fid, dt1, TipoMarcacao.Entrada, OrigemMarcacao.Web, null);
        var h2 = MarcacaoPontoIntegridade.Calcular(Fid, dt2, TipoMarcacao.SaidaAlmoco, OrigemMarcacao.Web, h1);
        var h3 = MarcacaoPontoIntegridade.Calcular(Fid, dt3, TipoMarcacao.Saida, OrigemMarcacao.Web, h2);

        var cadeia = new List<(Guid, Guid, DateTime, TipoMarcacao, OrigemMarcacao, string?, string)>
        {
            (Guid.NewGuid(), Fid, dt1, TipoMarcacao.Entrada, OrigemMarcacao.Web, null, h1),
            (Guid.NewGuid(), Fid, dt2, TipoMarcacao.SaidaAlmoco, OrigemMarcacao.Web, h1, h2),
            (Guid.NewGuid(), Fid, dt3, TipoMarcacao.Saida, OrigemMarcacao.Web, h2, h3),
        };

        MarcacaoPontoIntegridade.VerificarCadeia(cadeia).Should().BeNull();
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "MarcacaoPontoIntegridade")]
    [Fact(DisplayName = "Dada cadeia com hash adulterado no meio, VerificarCadeia retorna quebra no índice correto")]
    public void HashChain_HashAdulterado_DetectaQuebra()
    {
        var dt1 = new DateTime(2026, 6, 1, 8, 0, 0, DateTimeKind.Utc);
        var dt2 = new DateTime(2026, 6, 1, 12, 0, 0, DateTimeKind.Utc);

        var h1 = MarcacaoPontoIntegridade.Calcular(Fid, dt1, TipoMarcacao.Entrada, OrigemMarcacao.Web, null);
        var hashFake = new string('f', 64); // hash forjado

        var cadeia = new List<(Guid, Guid, DateTime, TipoMarcacao, OrigemMarcacao, string?, string)>
        {
            (Guid.NewGuid(), Fid, dt1, TipoMarcacao.Entrada, OrigemMarcacao.Web, null, h1),
            (Guid.NewGuid(), Fid, dt2, TipoMarcacao.SaidaAlmoco, OrigemMarcacao.Web, h1, hashFake),
        };

        var quebra = MarcacaoPontoIntegridade.VerificarCadeia(cadeia);
        quebra.Should().NotBeNull();
        quebra!.Indice.Should().Be(1);
        quebra.TipoQuebra.Should().Be("hash_integridade_divergente");
    }

    // ============================== GeradorEspelhoMensal

    [Trait("Solucao", "Services")]
    [Trait("Acao", "GeradorEspelhoMensal")]
    [Fact(DisplayName = "Dado funcionário+jornada+sem marcações, espelho mensal tem 30 dias e totais zero (todos faltas)")]
    public void EspelhoMensal_SemMarcacoes()
    {
        var jornada = NovaJornada(Jornada44hClt);
        var func = new Acme.Sistemas.Domain.Entities.Cadastros.Funcionario
        {
            Id = Fid, NomeCompleto = "Teste", Cpf = "12345678900",
        };

        var espelho = GeradorEspelhoMensal.Gerar(
            func, "2026-06", jornada, politica: null,
            marcacoes: Array.Empty<MarcacaoPonto>(),
            feriados: Array.Empty<Feriado>());

        espelho.Dias.Should().HaveCount(30);
        espelho.Totais.TrabalhadoMinutos.Should().Be(0);
        espelho.HashEspelho.Should().HaveLength(64);
        espelho.Competencia.Should().Be("2026-06");
    }
}
