# Design — rh-ponto-interno

## Cadeia de hash de integridade

```
MarcacaoPonto[n]                MarcacaoPonto[n+1]
┌────────────────┐              ┌────────────────┐
│ funcionarioId  │              │ funcionarioId  │
│ dataHora       │              │ dataHora       │
│ tipo           │              │ tipo           │
│ origem         │              │ origem         │
│ hashAnterior ──┼──────┐       │ hashAnterior ──┼──────┐
│ hashIntegridade│      │       │ hashIntegridade│      │
└───────┬────────┘      │       └───────┬────────┘      │
        │               │               │               │
        └───────────────┼───────────────┘               │
                        │                               │
                  SHA-256(campos|hashAnterior)          │
                        └───────────────────────────────┘

Adulteração em qualquer ponto invalida toda a cadeia a partir dali.
Verificação periódica via job: `JobVerificarIntegridadePonto`.
```

## Engine de cálculo

```
Inputs:
   ├── Funcionario (com sua jornada/escala vigente em cada dia)
   ├── Lista de MarcacaoPonto do período
   ├── Política de banco de horas vigente
   └── Calendário (feriados nacionais/regionais → W5 entrega tabela; aqui usa hardcoded)

  ▼
┌──────────────────────────────────────────────────────────┐
│  CalculadoraJornadaDiaria (por dia)                      │
│   1. Identifica janela esperada (jornada × dia da semana)│
│   2. Pareia batidas (E, SA, VA, S, ...) por sequência    │
│   3. Detecta gaps, sobras, atrasos, faltas               │
│   4. Calcula horas trabalhadas, intervalo, HE bruta      │
│   5. Retorna ResumoDia                                   │
└─────────────────────┬────────────────────────────────────┘
                      ▼
┌──────────────────────────────────────────────────────────┐
│  CalculadoraSaldoBancoHoras (por mês)                    │
│   Para cada ResumoDia × política:                         │
│   ├── se trabalhou MENOS que devido → -minutos no banco  │
│   ├── se trabalhou MAIS → +minutos no banco              │
│   ├── aplica fator de pagamento se configurado           │
│   └── emite MovimentoBancoHoras                          │
└─────────────────────┬────────────────────────────────────┘
                      ▼
┌──────────────────────────────────────────────────────────┐
│  GeradorEspelhoMensal                                    │
│   Linhas: dia | jornada esperada | batidas | trabalhado  │
│   | atraso | hora extra | saldo dia | saldo acumulado    │
└──────────────────────────────────────────────────────────┘
```

## Pareamento de batidas (heurística)

```
Batidas do dia (cronológicas): [b1, b2, b3, b4, ...]

Jornada padrão: [E, SA, VA, S]   (4 batidas)
Jornada 6x1:    [E, S]            (2 batidas)
Jornada 12x36:  [E, S]            (2 batidas, mas dia inteiro)

Pareamento:
  1. Conta batidas: se par e quantidade bate com jornada → pareia em sequência
  2. Se ímpar ou divergente:
     ├── chama "modo recuperação":
     │   ├── usa tipos declarados nas batidas (campo `tipo`) quando vierem
     │   ├── senão, infere por proximidade da janela esperada
     │   └── marca anomalias para revisão (status Invalida)
  3. Cada par (entrada, saída) vira intervalo trabalhado.
  4. Intervalo legal (CLT >6h trabalhadas exige 1h) detectado e logado.
```

## Estrutura do espelho mensal (JSON)

```json
{
  "funcionarioId": "...",
  "competencia": "2026-06",
  "jornadaVigente": { "nome": "44h CLT", "cargaSemanal": 44 },
  "politicaBancoHoras": { "nome": "Padrão", "limiteAcumular": 40 },
  "dias": [
    {
      "data": "2026-06-01",
      "diaSemana": "Seg",
      "janelaEsperada": { "entrada": "08:00", "saida": "17:30" },
      "batidas": [
        { "id": "...", "hora": "08:03", "tipo": "Entrada", "origem": "Web" },
        { "id": "...", "hora": "12:00", "tipo": "SaidaAlmoco", "origem": "Web" },
        { "id": "...", "hora": "13:30", "tipo": "VoltaAlmoco", "origem": "Web" },
        { "id": "...", "hora": "17:35", "tipo": "Saida", "origem": "Web" }
      ],
      "trabalhadoMinutos": 482,
      "esperadoMinutos": 510,
      "saldoDiaMinutos": -28,
      "anomalias": [],
      "atestadoUrl": null
    },
    ...
  ],
  "totais": {
    "diasUteis": 22,
    "diasTrabalhados": 21,
    "diasFalta": 1,
    "trabalhadoMinutos": 10460,
    "esperadoMinutos": 11220,
    "saldoMesMinutos": -760,
    "horasExtrasMinutos": 320,
    "saldoBancoAcumuladoMinutos": 1240
  },
  "hashEspelho": "sha256:abc..."
}
```

## Fluxo do app de fechamento

```
RH abre /rh/ponto/competencia/2026-06
  │
  ▼
Sistema lista funcionários com saldo do mês + alerta para anomalias
  │
  ▼
RH revisa anomalias, força inclusão de batida manual quando necessário
(cada manual exige justificativa + audit log)
  │
  ▼
RH clica "Fechar competência"
  │
  ▼
Sistema:
  ├── Trava edição de batidas do mês (status Fechado)
  ├── Gera espelho PDF para cada funcionário (async, queue)
  ├── Notifica funcionários (e-mail com link do espelho)
  ├── Persiste FechamentoPonto.fechado_em
  └── Disponibiliza dados para folha (W6) consumir
```

## Geração de PDF

Lib: **QuestPDF** (MIT, single-file, fluent API). Já é favorita no .NET 8.

Layout:
- Cabeçalho: logo do tenant, dados da empresa, dados do funcionário, competência, jornada vigente.
- Tabela diária: 31 linhas × colunas (data, dia, janela, batidas, trabalhado, esperado, saldo).
- Totais no rodapé.
- QR code com hash do espelho (verificação posterior).
- Marca d'água "GERENCIAL — NÃO SUBSTITUI PONTO OFICIAL PORTARIA 671" enquanto W4 não existe.

## Fila assíncrona para PDF em massa

```
POST /ponto/competencia/2026-06/fechar
  ├── Marca FechamentoPonto = Fechado
  └── Publica N mensagens "GerarEspelhoPdfMessage" em RabbitMQ
              │
              ▼
       Worker EspelhoPdfWorker (já temos NFeTransmissaoWorker como template)
              ├── Renderiza PDF (QuestPDF)
              ├── Upload S3/GED (chave: tenant/funcId/espelho/AAAAMM.pdf)
              ├── Notifica usuário (e-mail + bell do front)
              └── Atualiza progress no DB
```

## Permissions matrix

| Permissão | RH | Gestor | Funcionário | Admin tenant |
|-----------|:-:|:-:|:-:|:-:|
| rh-ponto:bater (próprio) | ✓ | ✓ | ✓ | ✓ |
| rh-ponto:listar próprio | ✓ | ✓ | ✓ | ✓ |
| rh-ponto:listar equipe | ✓ | ✓ (`gerir-equipe`) |   | ✓ |
| rh-ponto:listar todos | ✓ |   |   | ✓ |
| rh-ponto:ajustar (próprio) | ✓ | ✓ | ✓ | ✓ |
| rh-ponto:aprovar | ✓ | ✓ |   | ✓ |
| rh-ponto:manual | ✓ |   |   | ✓ |
| rh-ponto:fechar | ✓ |   |   | ✓ |
| rh-ponto:reabrir |   |   |   | ✓ |
| rh-banco-horas:gerir | ✓ |   |   | ✓ |
| rh-banco-horas:ver próprio | ✓ | ✓ | ✓ | ✓ |

## Tradeoffs

### Hash chain vs assinatura ICP-Brasil

- **Hash chain**: barato, sem cert digital, detecta adulteração mas não prova autoria.
- **ICP-Brasil**: exige cert do tenant + libs de assinatura → será W4.
- **Decisão**: hash chain agora; W4 substitui/complementa para conformidade legal.

### Pareamento heurístico de batidas

Funcionário pode esquecer de bater. Sistema **não** pode inventar batidas. Estratégia: pareia o que dá, marca o resto como anomalia, exige ajuste manual com workflow.

### Tipo de batida obrigatório?

- **Sim na web** (UI escolhe automaticamente baseado em última batida).
- **Inferido no mobile** (toca botão "Bater" único — sistema infere por última batida do dia).
- **Sempre persiste o tipo final** para auditoria.

### Banco de horas — diversas políticas

CLT permite acordos individuais e CCT pode prevalecer. Modelo de **política configurável** é necessário; default conservador é "sem banco — HE paga conforme jornada".

### Espelho PDF assíncrono

Para 1 funcionário fechar individual: síncrono (< 5s).
Para fechamento mensal de 1000 funcionários: queue obrigatório.

## Test strategy

- **Unit** (massivo): 20 fixtures de jornada+batidas+política × `CalculadoraJornadaDiaria` e `CalculadoraSaldoBancoHoras`.
  - 44h CLT padrão (8h45 dia).
  - 12x36 enfermagem.
  - 6x1 comércio.
  - Estagiário 6h.
  - Jornada noturna 22-06h (HE só identificada, adicional em W6).
  - Atrasos / faltas / saídas antecipadas.
  - HE com banco × HE sem banco × HE paga.
- **Integration**: fluxo `bater → ajustar → aprovar → fechar → gerar PDF → baixar PDF` ponta-a-ponta.
- **Property-based** (FsCheck opcional): batidas aleatórias num dia + jornada → invariante "soma trabalhado <= 24h".
- **Convention**: novas rotas `/api/v1/rh/ponto/*` aderem ao blueprint.
