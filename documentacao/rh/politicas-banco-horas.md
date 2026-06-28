# Políticas de banco de horas (rh-ponto-interno W2)

Documentação operacional do modelo de política e dos movimentos de banco de horas.

## Conceito

CLT permite que horas extras (HE) sejam **compensadas** dentro de um prazo combinado em
acordo individual/coletivo, em vez de pagas como adicional. O Atena modela isso como
**política configurável por tenant** (`BancoHorasPolitica`).

## Modelo da política

| Campo | Tipo | Default | Descrição |
|-------|------|---------|-----------|
| `nome` | VARCHAR(120) | (obrigatório) | Identificação da política (única por tenant) |
| `vigencia_inicio` / `vigencia_fim` | DATE | hoje / NULL | Período em que a política vale |
| `limite_horas_acumular` | DECIMAL(8,2) | 40 | Limite de horas acumuláveis; excedente expira |
| `prazo_compensacao_dias` | INT | 180 | Dias para compensar; expirado vai para folha |
| `permite_pagar_excedente` | BOOL | true | Se o saldo positivo pode ser pago em folha |
| `fator_pagamento` | DECIMAL(4,2) | 1.00 | Multiplicador no pagamento (1.5 = adicional 50%) |
| `ativo` | BOOL | true | Soft-disable sem deletar |

## Política default sugerida

```json
{
  "nome": "Padrão CLT",
  "limiteHorasAcumular": 40,
  "prazoCompensacaoDias": 180,
  "permitePagarExcedente": true,
  "fatorPagamento": 1.5
}
```

Política conservadora alternativa (sem banco de horas):

```json
{
  "nome": "Sem banco — HE paga em folha",
  "limiteHorasAcumular": 0,
  "prazoCompensacaoDias": 1,
  "permitePagarExcedente": true,
  "fatorPagamento": 1.5
}
```

## Movimentos (append-only)

Tabela `movimentos_banco_horas` é **imutável** (não há UPDATE). Cada movimento tem:

- `data` — dia do fato gerador
- `origem` — `Acumulo` / `Compensacao` / `Pagamento` / `Ajuste` / `Expiracao`
- `minutos` — positivo (entra) ou negativo (sai)
- `referencia_marcacao_id` — opcional, ligação à marcação que originou
- `competencia` — YYYY-MM (facilita query da folha)
- `observacao` — texto livre

Origens:

| Origem | Quem gera | Sinal | Descrição |
|--------|-----------|------:|-----------|
| `Acumulo` | Engine (`CalculadoraSaldoBancoHoras`) ou trabalho em feriado | + | Quando trabalhado > esperado num dia útil |
| `Compensacao` | RH via `POST /rh/banco-horas/compensar` | − | Consome saldo positivo |
| `Pagamento` | RH via `POST /rh/banco-horas/pagar` | − | Gera pendência para folha (W6) |
| `Ajuste` | RH manual (W3) | ± | Correções pontuais |
| `Expiracao` | Engine | − | Saldo acima do limite zera ao fim do mês |

## Casos típicos

### Trabalhador acumula 1h extra na segunda-feira

```
Dia 2026-06-01 (seg):
   Jornada: 09:00 esperadas
   Trabalhado: 10:00
   Saldo dia: +60min
   ResumoDia.SaldoMinutos = +60

CalculadoraSaldoBancoHoras emite:
   MovimentoBancoHoras { data:2026-06-01, origem:Acumulo, minutos:+60, competencia:"2026-06" }
```

### Trabalhador compensa 2h na sexta saindo mais cedo

```
RH chama:
   POST /rh/banco-horas/compensar
   { funcionarioId, data: 2026-06-05, minutos: 120, motivo: "Saída antecipada autorizada" }

Sistema grava:
   MovimentoBancoHoras { data:2026-06-05, origem:Compensacao, minutos:-120, competencia:"2026-06" }
```

### Saldo passa de 40h (limite) → expira

```
Saldo acumulado no mês: 56h (3360min)
Limite política: 40h (2400min)
Excedente: 960min

CalculadoraSaldoBancoHoras emite:
   MovimentoBancoHoras { data:último dia do mês, origem:Expiracao, minutos:-960,
                         observacao: "Excedente acima do limite (40h) zerado conforme política." }

Saldo final: 2400min (40h, exato no limite)
```

### RH paga 10h do banco em folha

```
POST /rh/banco-horas/pagar
{ funcionarioId, competencia: "2026-06", minutos: 600 }

Sistema:
   1. Grava MovimentoBancoHoras { origem:Pagamento, minutos:-600 }
   2. Retorna pendência: "PendFolha:{funcionarioId}:2026-06"
   3. Folha (W6) consulta pendências da competência e gera rubrica de HE paga
      com valor = (salário_hora × 600/60 × fator_pagamento da política)
```

## Boas práticas

- **Crie pelo menos uma política ativa por tenant** (default "Padrão CLT"). Sem política,
  o engine usa limite 40h hardcoded como fallback (ver `CalculadoraSaldoBancoHoras`).
- **Não tenha múltiplas políticas vigentes simultaneamente** — pode causar ambiguidade.
  Use `vigencia_fim` para encerrar políticas antigas.
- **Anexe a política ao funcionário em `banco_horas_saldo.politica_id`** — assim cada
  competência usa a política certa mesmo que a configuração geral mude depois.
- **Auditoria**: cada criação de movimento é auditada (`AuditBehavior`); cada decisão de
  ajuste (aprovar/rejeitar) também. Use `/api/v1/auditoria/historico/MarcacaoPonto/{id}` para
  ver o histórico de mudanças de uma batida.
