# Ponto interno (W2 — rh-ponto-interno)

Manual operacional do módulo de ponto **gerencial** do Atena. Cobre:
batidas, ajustes, espelho mensal, banco de horas e fechamento de competência.

> ⚠ **Importante**: este módulo entrega ponto **gerencial**, suficiente para empresas
> não fiscalizadas pela Portaria 671 e para servir de base ao cálculo da folha em W6.
> Para **conformidade legal Portaria 671** (REP-A com assinatura ICP-Brasil + AFD/AEJ +
> reprovação SREI), aguardar **W4 — rh-ponto-oficial-671**.

## Conceitos

- **Marcação de ponto**: cada batida (`MarcacaoPonto`) é encadeada por hash SHA-256 à
  marcação anterior do mesmo funcionário. Adulteração quebra a cadeia — detectado pelo
  `JobVerificarIntegridadePontoWorker` (hosted noturno).
- **Ajuste**: funcionário pode solicitar inclusão/alteração/exclusão de batida ou anexar
  justificativa. Workflow: `Pendente → Aprovado/Rejeitado` (gestor com `rh-ponto:aprovar-ponto`).
- **Espelho mensal**: estrutura JSON + PDF (QuestPDF) com saldo do mês, banco de horas,
  HE bruta, anomalias. Hash do espelho impresso no rodapé.
- **Banco de horas**: política configurável por tenant (limite acumular, prazo compensação,
  fator pagamento). Cada movimento (Acumulo/Compensacao/Pagamento/Ajuste/Expiracao) é
  append-only.
- **Fechamento**: trava edição de marcações da competência. Reabertura só por admin tenant.

## Fluxo principal

```
   [Funcionário]
        │
        ├── POST /rh/ponto/bater       (próprio, infere tipo da última batida do dia)
        ├── POST /rh/ponto/ajustes     (solicita ajuste se errou)
        └── GET  /rh/ponto/proprio     (vê próprias batidas)

   [Gestor com rh-ponto:aprovar-ponto]
        ├── GET  /rh/ponto/ajustes/pendentes
        ├── POST /rh/ponto/ajustes/{id}/aprovar   (gera nova MarcacaoPonto)
        └── POST /rh/ponto/ajustes/{id}/rejeitar  (notifica funcionário)

   [RH com rh-ponto:fechar-competencia]
        ├── GET  /rh/ponto/espelho                (qualquer funcionário)
        ├── GET  /rh/ponto/espelho.pdf            (download PDF)
        ├── POST /rh/ponto/manual                 (inclui batida manualmente, sempre auditado)
        ├── POST /rh/ponto/competencia/fechar     (trava + gera espelho)
        └── GET  /rh/ponto/competencia/{c}/status

   [Admin tenant com rh-ponto:reabrir-competencia]
        └── POST /rh/ponto/competencia/reabrir    (folha do W6 fica marcada como desatualizada)
```

## Endpoints REST

### Marcações
| Método | Rota | Permissão | Descrição |
|--------|------|-----------|-----------|
| POST | `/api/v1/rh/ponto/bater` | `rh-ponto:bater-ponto` | Bater próprio ponto (tipo inferido) |
| POST | `/api/v1/rh/ponto/manual` | `rh-ponto:editar` | RH inclui batida manual auditada |
| GET | `/api/v1/rh/ponto/proprio?dataInicio=&dataFim=` | `rh-ponto:ler` | Lista próprias batidas |
| GET | `/api/v1/rh/ponto/equipe/{funcId}?dataInicio=&dataFim=` | `rh-ponto:gerir-equipe` | Gestor vê equipe |

### Ajustes
| POST | `/api/v1/rh/ponto/ajustes` | `rh-ponto:ajustar-ponto` | Solicita ajuste |
| POST | `/api/v1/rh/ponto/ajustes/{id}/aprovar` | `rh-ponto:aprovar-ponto` | Aprova + gera nova marcação |
| POST | `/api/v1/rh/ponto/ajustes/{id}/rejeitar` | `rh-ponto:aprovar-ponto` | Rejeita com justificativa |
| GET | `/api/v1/rh/ponto/ajustes/pendentes` | `rh-ponto:aprovar-ponto` | Lista pendentes |

### Espelho
| GET | `/api/v1/rh/ponto/espelho?funcionarioId=&competencia=YYYY-MM` | `rh-ponto:ler` | JSON estruturado |
| GET | `/api/v1/rh/ponto/espelho.pdf?...` | `rh-ponto:ler` | Download PDF (QuestPDF) |

### Fechamento
| POST | `/api/v1/rh/ponto/competencia/fechar` | `rh-ponto:fechar-competencia` | Fecha competência |
| POST | `/api/v1/rh/ponto/competencia/reabrir` | `rh-ponto:reabrir-competencia` | Admin reabre com motivo |
| GET | `/api/v1/rh/ponto/competencia/{c}/status` | `rh-ponto:ler` | Lista status por funcionário |

### Banco de horas
| GET | `/api/v1/rh/banco-horas/saldo?funcionarioId=&competencia=` | `rh-banco-horas:ler` | Saldo agregado |
| GET | `/api/v1/rh/banco-horas/movimentos?funcionarioId=&competencia=` | `rh-banco-horas:ler` | Lista movimentos |
| POST | `/api/v1/rh/banco-horas/compensar` | `rh-banco-horas:editar` | Compensa horas |
| POST | `/api/v1/rh/banco-horas/pagar` | `rh-banco-horas:editar` | Paga saldo (pendência folha W6) |
| GET | `/api/v1/rh/banco-horas/politicas` | `rh-politicas-ponto:ler` | Lista políticas |
| POST | `/api/v1/rh/banco-horas/politicas` | `rh-politicas-ponto:criar` | Cria política |

## Hash de integridade

Cada `MarcacaoPonto` armazena:
- `hash_anterior` — hash da marcação anterior do mesmo funcionário (NULL se primeira)
- `hash_integridade` — `SHA-256(funcionarioId | dataHora ISO 8601 | tipo | origem | hash_anterior)`

Adulteração em qualquer linha quebra a cadeia. `JobVerificarIntegridadePontoWorker` roda a
cada 24h e grava `AuditLog` quando detecta `MarcacaoPontoIntegridadeViolada`. Logs estruturados
NLog identificam funcionário, marcação e tipo de quebra (hash anterior divergente ou hash atual).

## Frontend (`/rh/ponto/*`)

- **Meu ponto** (`/rh/ponto/meu-ponto`) — botão grande "Bater" + tabela da semana
- **Espelho mensal** (`/rh/ponto/espelho`) — calendário com saldo + botão download PDF
- **Aprovações pendentes** (`/rh/ponto/aprovacoes`) — lista com aprovar/rejeitar inline (gestor)
- **Banco de horas** (`/rh/ponto/banco-horas`) — saldo + extrato de movimentos
- **Políticas BH** (`/rh/ponto/politicas`) — CRUD de políticas
- **Fechamento** (`/rh/ponto/fechamento`) — RH lista status + fecha/reabre

## Engine de cálculo (`Services/V1/Rh/Ponto/Engine/`)

- **`PareadorBatidas`** — pareia batidas cronológicas em intervalos trabalhados; detecta
  anomalias (quantidade ímpar, falta de intervalo CLT em jornada > 6h).
- **`CalculadoraJornadaDiaria`** — lê jornada vigente, calcula trabalhado/esperado/saldo/HE
  por dia. Função pura (16 fixtures unit cobrem CLT/estágio/feriado/atraso/HE).
- **`CalculadoraSaldoBancoHoras`** — agrega ResumoDia em movimentos do banco; aplica
  expiração no limite da política.
- **`GeradorEspelhoMensal`** — produz JSON do espelho com hash SHA-256 do conteúdo.
- **`GeradorEspelhoPdfQuestPdf`** — renderiza PDF (QuestPDF Community) com marca d'água
  "GERENCIAL — NÃO SUBSTITUI PONTO OFICIAL PORTARIA 671".
- **`MarcacaoPontoIntegridade`** — hash-chain helper + `VerificarCadeia`.

## E-mails

Templates em `PontoEmailTemplates`:
1. **AjusteDecidido** — notifica funcionário quando seu ajuste é aprovado/rejeitado
2. **EspelhoDisponivel** — notifica funcionário quando espelho mensal foi gerado
3. **DigestPendentesGestor** — digest diário ao gestor com até 10 ajustes pendentes

HTML responsivo (Gmail/Outlook) + versão texto. Disparo via `EmailQueueService` (RabbitMQ + MailKit SMTP).

## Próximas evoluções

- **W3 (rh-mobile-maui)**: app .NET MAUI bate ponto via `/api/v1/rh/ponto/bater` com biometria.
- **W4 (rh-ponto-oficial-671)**: ICP-Brasil + AFD/AEJ + reprovação SREI.
- **W6 (rh-folha-engine)**: consome `historico_salarios` + saldo banco horas + escala para calcular folha.

## Limitações conhecidas

- PDF em massa (todos funcionários de uma competência) é síncrono — para tenants > 100 funcionários,
  considerar implementar `EspelhoPdfWorker` RabbitMQ (template: `NFeTransmissaoWorker`).
- Feriados regionais (estaduais/municipais) ainda não populados — apenas 14 nacionais inline.
  Upload via endpoint admin opt-in virá em W5.
- Notificação real-time no bell front-end depende de polling do `NotificacaoService` existente
  (entrega típica < 30s).
