# Design — rh-eventos-mes

## Cálculo de férias

```
Período aquisitivo: 12 meses a partir da admissão (e ciclos depois)
Dias de direito (CLT art. 130):
  0-5 faltas      → 30 dias
  6-14 faltas     → 24 dias
  15-23 faltas    → 18 dias
  24-32 faltas    → 12 dias
  >32 faltas      → perde direito

Venda (abono pecuniário):
  até 10 dias (1/3 do total)
  valor: salário diário × dias vendidos × 1.333 (1 + 1/3)

Folha de férias (paga até 2 dias antes do início):
  Salário do mês (proporcional aos dias gozados se < 30)
  + 1/3 constitucional sobre salário
  + Abono pecuniário (se vendeu)
  + Adiantamento 13º (se solicitou)
  - INSS sobre (salário + 1/3) — incide
  - IRRF sobre (salário + 1/3) — incide
  Líquido = soma

Restante do mês (se gozo parcial) entra na folha normal.
```

## Cálculo do 13º

```
1ª parcela (até 30/nov):
  = (salário vigente em novembro / 12) × meses_trabalhados_no_ano_até_novembro × 0.5
  Sem desconto

2ª parcela (até 20/dez):
  total_13o = (salário vigente em dezembro / 12) × meses_trabalhados_no_ano
  base_inss = total_13o
  inss = aplicaTabelaInss(base_inss)  -- tabela separada de 13º (mesma do mês, mas integral)
  base_irrf = total_13o - inss
  irrf = aplicaTabelaIrrf(base_irrf, dependentes)
  2ª parcela = total_13o - 1ª parcela - inss - irrf

"Mês trabalhado": ≥ 15 dias no mês.
```

## Engine especializado

Cada engine reusa `ContextoFuncionarioFolha`/`EngineFolhaMensal` mas pré-popula contexto diferente:

```csharp
public sealed class EngineFolhaFerias
{
    public async Task<HoleriteFuncionario> CalcularAsync(Guid funcId, Guid feriasId)
    {
        var ferias = await _repo.ObterAsync(feriasId);
        var ctx = await _prep.PrepararContextoFeriasAsync(funcId, ferias);

        // Rubricas específicas de férias
        ctx.Add("F01-SAL-FERIAS",  ctx.SalarioFeriasProporcional());
        ctx.Add("F02-1-3-CONST",   ctx.UmTercoConstitucional());
        if (ferias.DiasVendidos > 0)
            ctx.Add("F03-ABONO-PEC", ctx.AbonoPecuniarioComUmTerco());
        if (ferias.AdiantamentoDecimo3o)
            ctx.Add("F04-ADTO-13",  ctx.Adiantamento13o());

        // Descontos
        ctx.Add("F100-INSS-FER",  -ctx.InssSobreFerias());
        ctx.Add("F110-IRRF-FER",  -ctx.IrrfSobreFerias());

        // Consolida igual W6
        return ...;
    }
}
```

## Período aquisitivo

```sql
CREATE TABLE ferias (
  id, tenant_id, funcionario_id,
  periodo_aquisitivo_inicio DATE NOT NULL,
  periodo_aquisitivo_fim DATE NOT NULL,
  dias_direito INT NOT NULL DEFAULT 30,
  dias_pendentes INT NOT NULL,
  data_inicio_gozo DATE,
  data_fim_gozo DATE,
  dias_gozados INT,
  dias_vendidos INT,
  abono_pecuniario_valor DECIMAL(10,2),
  adiantamento_13o BOOLEAN,
  status ENUM('Pendente','Programada','EmGozo','Concluida','Vencida') NOT NULL,
  folha_id CHAR(36),
  programada_em DATETIME, programada_por CHAR(36),
  ...
);
```

Job noturno:
```
foreach funcionário ativo:
  Último ferias existente ou data admissão → calcula período aquisitivo atual
  se data atual >= fim do período E não tem férias programadas:
    cria Ferias.Pendente
    notifica RH
  se data atual > fim + 12 meses (CLT prazo p/ gozar):
    cria Ferias.Vencida
    alerta CRÍTICO (empresa precisa pagar em dobro)
```

## Afastamentos — split empresa/INSS

```
Atestado / doença:
  dias 1-15: empresa paga normal
  dia 16 em diante: INSS paga (empresa não paga, mas mantém vínculo)

Quando dia 15 chega:
  - Sistema gera ticket "abrir benefício INSS"
  - RH precisa solicitar perícia ao INSS
  - Em folha do mês: zero dias empresa após dia 15

Maternidade:
  - 120 dias INSS (rede empresa paga depois para INSS recuperar)
  - +60 dias empresa-cidadã (opcional, com benefício fiscal)

Paternidade: 5 dias empresa (CLT) ou 20 dias empresa-cidadã.

Outras licenças remuneradas (gala, nojo, doação sangue): empresa paga, dias listados em CLT art. 473.
```

## Aviso de férias PDF

Documento obrigatório (CLT art. 135), entregue ao funcionário 30 dias antes do início:
```
AVISO DE FÉRIAS

Empregado: ...
Período aquisitivo: ... a ...
Dias de direito: 30
Dias a gozar: 30 (ou 20 + 10 vendidos)
Início do gozo: DD/MM/AAAA
Retorno: DD/MM/AAAA
Pagamento: DD/MM/AAAA (2 dias antes)

Assinatura empregado: ____________   Data: __/__/____
Assinatura empresa:    ____________
```

## Tradeoffs

### 13º como folha separada vs rubrica na folha normal?

Folha separada (`FolhaMensal.tipo = Decimo3oParcelaX`) — IRRF retido separadamente, holerite distinto, eSocial S-1200 separa também.

### Pagar férias como folha avulsa vs integrar com folha normal?

Folha avulsa quando início de gozo ≠ início do mês. Quando coincide, pode integrar.

### Job automático de geração?

- Férias.Pendente: sim, no fim de cada mês.
- 13º: sim, dia 15/nov e 15/dez (deixa 5-15 dias para RH processar).
- Aviso de férias vencendo: sim, alerta diário.

## Test strategy

- Unit: dias_direito por número de faltas (5 cenários).
- Unit: cálculo 13º integral (12 meses), proporcional (6, 9 meses), com aumento no meio.
- Unit: ferias com venda, com adiantamento 13º.
- Integration: programar férias → gera aviso PDF + folha avulsa correta.
- Integration: job de pendência cria férias corretamente para funcionário com período completo.
