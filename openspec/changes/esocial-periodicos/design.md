# Design — esocial-periodicos

## S-1200 (Remuneração) — mapeamento de holerite

```
HoleriteFuncionario               S-1200
─────────────────────             ──────
funcionario_id              →     ideTrabalhador.cpfTrab
folha.competencia           →     ideEvento.perApur
folha.empregador_id         →     ideEmpregador.nrInsc
rubricas_calculadas         →     dmDev[].infoPerApur.ideEstabLot.itensRemun[]
   cada rubrica:                       codRubr (mapeado natureza eSocial)
                                       qtdRubr, fatorRubr, vrUnit, vrRubr
                                       indApurIR, indApurIRRRA
baseInss                    →     dmDev.infoAgNocivo
baseIrrf                    →     infoIRComplem
```

## S-1210 (Pagamento)

```
ContaPagar pago             →     S-1210
ContaPagar.referencia_id    →     ideTrabalhador.cpfTrab
ContaPagar.valor_pago       →     infoPgto.dtPgto + vrLiq
folha.id                    →     ideDmDev (referência S-1200 original)
```

## S-1299 (Fechamento)

Header simples:
```xml
<evtFechaEvPer>
  <ideEvento perApur="2026-06" .../>
  <ideEmpregador .../>
  <ideRespInf cpfResp="..." nmResp="..."/>
  <infoFech evtRemun="S" evtPgtos="S" evtAqProd="N" evtComProd="N" evtContratAvNP="N" evtInfoComplPer="N" compSemMovto=""/>
</evtFechaEvPer>
```

## Orquestrador da competência

```csharp
public sealed class OrquestradorPeriodicos
{
    public async Task<StatusCompetencia> StatusAsync(Guid empregadorId, string competencia)
    {
        var s1200Pendentes = await _eventoRepo.ContarPorStatusAsync(empregadorId, "S-1200", competencia, "!Aceito");
        var s1210Pendentes = await _eventoRepo.ContarPorStatusAsync(empregadorId, "S-1210", competencia, "!Aceito");
        var s1299 = await _eventoRepo.UltimoAsync(empregadorId, "S-1299", competencia);

        return new StatusCompetencia(
            S1200Pendentes: s1200Pendentes,
            S1210Pendentes: s1210Pendentes,
            S1299: s1299?.Status,
            PodeFechar: s1200Pendentes == 0 && s1210Pendentes == 0 && s1299 == null);
    }
}
```

## Retificação encadeada

```
Cenário: erro detectado em S-1200 já Aceito de competência fechada

1. Reabrir competência: S-3000 do S-1299 → eSocial reabre.
2. Retificar S-1200: S-1200 indRetif=2 com correção.
3. Eventualmente retificar S-1210 se valor pagamento mudou.
4. Re-fechar: novo S-1299.
```

Sequência implementada via `OrquestradorRetificacaoCompetencia`.

## Test strategy

- Unit: builder S-1200 com fixture de holerite complexo (várias rubricas, dependentes, pensão)
- Unit: detector de mudança em holerite que dispara retificação
- Integration: ciclo S-1200 + S-1210 + S-1299 5 funcs em Restrita
- Integration: retificação S-1200 funciona
- Integration: reabrir competência via S-3000 do S-1299
