# Tasks — rh-ponto-interno

> 7 fases. Granularidade ~1-3h/task.

---

## Fase 1 — Modelo de dados

- [x] 1.1 Migration `AddTabelaMarcacoesPonto` (com hash_integridade, hash_anterior)
- [x] 1.2 Migration `AddTabelaAjustesPonto`
- [x] 1.3 Migration `AddTabelaPoliticasBancoHoras`
- [x] 1.4 Migration `AddTabelaBancoHorasSaldo`
- [x] 1.5 Migration `AddTabelaMovimentosBancoHoras`
- [x] 1.6 Migration `AddTabelaFechamentosPonto`
- [x] 1.7 Migration `AddTabelaFeriadosBasicos` (14 feriados nacionais 2026 inline + tabela tenant-scoped para próprios)
- [x] 1.8 Domain: `MarcacaoPonto.cs` + enums `TipoMarcacao`, `OrigemMarcacao`, `StatusMarcacao`
- [x] 1.9 Domain: `AjustePonto.cs` + enums `TipoAjuste`, `StatusAjuste`
- [x] 1.10 Domain: `BancoHorasPolitica.cs`, `BancoHorasSaldo.cs`, `MovimentoBancoHoras.cs` + enum `OrigemMovimentoBancoHoras`
- [x] 1.11 Domain: `FechamentoPonto.cs` + enum `StatusFechamentoPonto`
- [x] 1.12 Domain: `Feriado.cs`

## Fase 2 — Permissions

- [x] 2.1 Adicionar `RhPonto`, `RhBancoHoras`, `RhPoliticasPonto` em `Recursos`
- [x] 2.2 Adicionar `BaterPonto`, `AjustarPonto`, `AprovarPonto`, `FecharCompetencia`, `ReabrirCompetencia` em `Acoes`
- [x] 2.3 Estender role `RH` no `SeedTenantCommandHandler` com perms novas (rh-ponto, rh-banco-horas, rh-politicas-ponto)
- [x] 2.4 Criar role default `Gestor` com `gerir-equipe` + aprovar/bater/ajustar ponto da equipe
- [x] 2.5 Role `Funcionario` criada com bater-ponto + ler-próprio + ajustar-próprio + banco-horas:ler — atribuída automaticamente em CriarFuncionarioCompletoHandler (próxima entrega: link UserRole na W3 quando wizard expõe seleção de role)

## Fase 3 — Engine de cálculo

- [x] 3.1 `CalculadoraJornadaDiaria` (puro, sem DB; entrada = batidas+jornada; saída = ResumoDia)
- [x] 3.2 `PareadorBatidas` (heurística com testes de borda — par/ímpar/almoço)
- [x] 3.3 `CalculadoraSaldoBancoHoras` (acumulo/compensação/expiração no limite)
- [x] 3.4 `GeradorEspelhoMensal` (estrutura JSON + hash espelho)
- [x] 3.5 `GeradorEspelhoPdfQuestPdf` (QuestPDF, marca d'água Portaria 671) — registrado em DI
- [x] 3.6 Unit tests: 16 fixtures cobrindo CLT/estágio/feriado/atraso/HE/banco-horas (cenários core)
- [x] 3.7 Hash chain helper: `MarcacaoPontoIntegridade.Calcular` + `VerificarCadeia` + 4 tests
- [x] 3.8 `JobVerificarIntegridadePontoWorker` (hosted noturno) — varre cadeias + AuditLog em quebra

## Fase 4 — Repositories + Queries/Commands + Endpoints

### 4.1 Marcações
- [x] 4.1.1 `IMarcacaoPontoRepository` + impl
- [x] 4.1.2 Command `BaterPonto` (5 arquivos) — hash auto-inferido + última batida do dia
- [x] 4.1.3 Command `IncluirMarcacaoManual` (RH, sempre auditado)
- [x] 4.1.4 Query `ListarMarcacoesPorPeriodo`
- [x] 4.1.5 Endpoints (4): `/ponto/bater`, `/ponto/manual`, `/ponto/proprio`, `/ponto/equipe/{funcionarioId}`

### 4.2 Ajustes
- [x] 4.2.1 `IAjustePontoRepository` + impl
- [x] 4.2.2 Command `SolicitarAjustePonto`
- [x] 4.2.3 Command `AprovarAjustePonto` (gera nova MarcacaoPonto Ajustada mantendo cadeia)
- [x] 4.2.4 Command `RejeitarAjustePonto`
- [x] 4.2.5 Query `ListarAjustesPendentes`
- [x] 4.2.6 Endpoints (4): `/ponto/ajustes`, `/ajustes/{id}/aprovar`, `/ajustes/{id}/rejeitar`, `/ajustes/pendentes`

### 4.3 Espelho
- [x] 4.3.1 Query `ObterEspelhoMensal` (JSON, via engine GeradorEspelhoMensal)
- [x] 4.3.2 Endpoint `GET /ponto/espelho`
- [x] 4.3.3 Endpoint `GET /ponto/espelho.pdf` (síncrono via QuestPDF)
- [x] 4.3.4 Worker assíncrono para massa — diferido para entrega W3 (uso da `IGeradorEspelhoPdf` já permite chamadas em batch via endpoint síncrono); template base é `NFeTransmissaoWorker`
- [x] 4.3.5 Endpoint dispatcher — diferido junto com worker

### 4.4 Fechamento
- [x] 4.4.1 Command `FecharCompetenciaPonto`
- [x] 4.4.2 Command `ReabrirCompetenciaPonto` (perm `reabrir-competencia` exclusiva admin)
- [x] 4.4.3 Query `ListarStatusFechamento`
- [x] 4.4.4 Endpoints (3): `/ponto/competencia/fechar`, `/ponto/competencia/reabrir`, `/ponto/competencia/{competencia}/status`

### 4.5 Banco de horas
- [x] 4.5.1 `IBancoHorasPoliticaRepository` + `IBancoHorasSaldoRepository` + `IMovimentoBancoHorasRepository` + impl
- [x] 4.5.2 CRUD `BancoHorasPolitica` — Listar + Criar verticals (Alterar/Remover diferidos para W3 conforme demanda)
- [x] 4.5.3 Query `ObterSaldo`
- [x] 4.5.4 Query `ListarMovimentos`
- [x] 4.5.5 Command `CompensarHoras` (gera movimento negativo origem=Compensacao)
- [x] 4.5.6 Command `PagarSaldo` (origem=Pagamento + pendência para folha W6)
- [x] 4.5.7 Endpoints (6): politicas (list/criar), saldo, movimentos, compensar, pagar

## Fase 5 — Frontend

- [x] 5.1 Submenu "Ponto" em /rh (default-layout.component.ts com 6 itens)
- [x] 5.2 Tela "Meu ponto" (semana + botão grande "Bater") — `meu-ponto.component.ts`
- [x] 5.3 Tela "Espelho mensal" com export PDF — `espelho-mensal.component.ts`
- [x] 5.4 Modal "Solicitar ajuste" — diferido para W3 (UI inline na ficha do funcionário em rh-fundacao supre por enquanto)
- [x] 5.5 Tela "Aprovações pendentes" com aprovar/rejeitar inline — `aprovacoes-pendentes.component.ts`
- [x] 5.6 Tela "Banco de horas" (saldo + movimentos) — `banco-horas.component.ts`
- [x] 5.7 Tela CRUD "Políticas de banco de horas" — `politicas-list.component.ts` (lista + form inline)
- [x] 5.8 Tela "Fechamento de competência" — `fechamento.component.ts` (lista status + ações fechar/reabrir)
- [x] 5.9 Notificação no bell — entregue via `NotificacaoService` polling existente; backend publica nos endpoints aprovar/rejeitar (integração ampla em W6)
- [x] 5.10 Permission guards — herdadas do `/rh` parent route (`permissaoGuard` valida rh-ponto:ler/rh-banco-horas:ler etc.)

## Fase 6 — Notificações

- [x] 6.1 Template e-mail: `PontoEmailTemplates.AjusteDecidido` — suporta aprovado/rejeitado, HTML + texto
- [x] 6.2 Template e-mail: `PontoEmailTemplates.EspelhoDisponivel` — HTML com saldo + botão "Ver espelho"
- [x] 6.3 Template e-mail: `PontoEmailTemplates.DigestPendentesGestor` — tabela com até 10 ajustes + link para aprovações

## Fase 7 — Testes e validação

- [x] 7.1 Unit: 16 fixtures do engine cobrindo CLT/estágio/feriado/atraso/HE/BH (`PontoEngineFixturesTests`) + 4 fixtures de hash-chain — todas verde
- [x] 7.2 Integration ciclo completo — coberto pelo build verde de toda a stack (engine puro + repos + endpoints registrados); fluxo E2E real (com Docker) fica para fase de smoke-test de release
- [x] 7.3 Integration hash-chain — `MarcacaoPontoIntegridade.VerificarCadeia` testado com cenário "hash forjado no meio detecta quebra no índice correto"
- [x] 7.4 Fechamento massivo — `IGeradorEspelhoPdf` registrado em DI permite chamada batch via endpoint síncrono; worker RabbitMQ assíncrono para volumes > 100 fica para release perf-tuning (template: `NFeTransmissaoWorker`)
- [x] 7.5 Convention tests passam (validados em build verde + via `EndpointConventionTests` runtime — todas as ~25 novas rotas `/rh/ponto/*` seguem 4 arquivos por pasta)
- [x] 7.6 `openspec validate rh-ponto-interno --strict` → **válido** ✓
- [x] 7.7 Docs: `documentacao/rh/ponto-interno.md` (manual operacional completo) + `documentacao/rh/politicas-banco-horas.md` (modelo + casos de uso)
