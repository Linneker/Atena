# Tasks — rh-ponto-interno

> 7 fases. Granularidade ~1-3h/task.

---

## Fase 1 — Modelo de dados

- [ ] 1.1 Migration `AddTabelaMarcacoesPonto` (com hash_integridade, hash_anterior)
- [ ] 1.2 Migration `AddTabelaAjustesPonto`
- [ ] 1.3 Migration `AddTabelaPoliticasBancoHoras`
- [ ] 1.4 Migration `AddTabelaBancoHorasSaldo`
- [ ] 1.5 Migration `AddTabelaMovimentosBancoHoras`
- [ ] 1.6 Migration `AddTabelaFechamentosPonto`
- [ ] 1.7 Migration `AddTabelaFeriadosBasicos` (subset nacional ~14, opt-in completo via upload em W5)
- [ ] 1.8 Domain: `MarcacaoPonto.cs` + enums `TipoMarcacao`, `OrigemMarcacao`, `StatusMarcacao`
- [ ] 1.9 Domain: `AjustePonto.cs` + enums `TipoAjuste`, `StatusAjuste`
- [ ] 1.10 Domain: `BancoHorasPolitica.cs`, `BancoHorasSaldo.cs`, `MovimentoBancoHoras.cs` + enum `OrigemMovimento`
- [ ] 1.11 Domain: `FechamentoPonto.cs` + enum `StatusFechamento`
- [ ] 1.12 Domain: `Feriado.cs`

## Fase 2 — Permissions

- [ ] 2.1 Adicionar `RhPonto`, `RhBancoHoras`, `RhPoliticasPonto` em `Recursos`
- [ ] 2.2 Adicionar `BaterPonto`, `AjustarPonto`, `AprovarPonto`, `FecharCompetencia`, `ReabrirCompetencia` em `Acoes`
- [ ] 2.3 Estender role `RH` no `SeedTenantCommandHandler` com perms novas
- [ ] 2.4 Criar role default `Gestor` (ou estender role `Operador`) com `gerir-equipe`
- [ ] 2.5 Auto-atribuir `rh-ponto:bater-ponto` + `rh-ponto:listar-proprio` quando `Funcionario` é criado

## Fase 3 — Engine de cálculo

- [ ] 3.1 `CalculadoraJornadaDiaria` (puro, sem DB; entrada = batidas+jornada; saída = ResumoDia)
- [ ] 3.2 `PareadorBatidas` (heurística com testes de borda)
- [ ] 3.3 `CalculadoraSaldoBancoHoras`
- [ ] 3.4 `GeradorEspelhoMensal` (estrutura JSON)
- [ ] 3.5 `GeradorEspelhoPdf` (QuestPDF)
- [ ] 3.6 Unit tests: 20 fixtures (1 por cenário) com expects calculados à mão
- [ ] 3.7 Hash chain helper: `MarcacaoPontoIntegridade.Calcular(prevHash, dados)` + tests
- [ ] 3.8 `JobVerificarIntegridadePonto` (hosted service noturno) + tests

## Fase 4 — Repositories + Queries/Commands + Endpoints

### 4.1 Marcações
- [ ] 4.1.1 `IMarcacaoPontoRepository` + impl
- [ ] 4.1.2 Command `BaterPonto` (5 arquivos) — calcula hashes, valida sequência
- [ ] 4.1.3 Command `IncluirMarcacaoManual` (RH)
- [ ] 4.1.4 Query `ListarMarcacoesPorPeriodo` (próprio/equipe/todos)
- [ ] 4.1.5 Endpoints (`/ponto/bater`, `/ponto/manual`, `/ponto/proprio`, `/ponto/equipe`)

### 4.2 Ajustes
- [ ] 4.2.1 `IAjustePontoRepository` + impl
- [ ] 4.2.2 Command `SolicitarAjustePonto`
- [ ] 4.2.3 Command `AprovarAjustePonto` (cria nova `MarcacaoPonto.Ajustada` mantendo cadeia)
- [ ] 4.2.4 Command `RejeitarAjustePonto`
- [ ] 4.2.5 Query `ListarAjustesPendentes`
- [ ] 4.2.6 Endpoints

### 4.3 Espelho
- [ ] 4.3.1 Query `ObterEspelhoMensal` (JSON)
- [ ] 4.3.2 Endpoint `GET /ponto/espelho`
- [ ] 4.3.3 Endpoint `GET /ponto/espelho.pdf` (síncrono individual)
- [ ] 4.3.4 Worker `EspelhoPdfWorker` (RabbitMQ) para geração em massa
- [ ] 4.3.5 Endpoint `POST /ponto/competencia/{ymd}/gerar-espelhos` (dispara worker)

### 4.4 Fechamento
- [ ] 4.4.1 Command `FecharCompetenciaPonto`
- [ ] 4.4.2 Command `ReabrirCompetenciaPonto` (admin tenant)
- [ ] 4.4.3 Query `ListarStatusFechamento`
- [ ] 4.4.4 Endpoints

### 4.5 Banco de horas
- [ ] 4.5.1 `IBancoHorasRepository` + impl
- [ ] 4.5.2 CRUD `BancoHorasPolitica` (5 verticals)
- [ ] 4.5.3 Query `ObterSaldoBancoHoras`
- [ ] 4.5.4 Query `ListarMovimentosBancoHoras`
- [ ] 4.5.5 Command `CompensarHorasBanco`
- [ ] 4.5.6 Command `PagarSaldoBancoHoras` (cria pendência para folha em W6)
- [ ] 4.5.7 Endpoints

## Fase 5 — Frontend

- [ ] 5.1 Submenu "Ponto" em /rh
- [ ] 5.2 Tela "Meu ponto" (semana + botão grande "Bater")
- [ ] 5.3 Tela "Espelho mensal" (calendário) com export PDF
- [ ] 5.4 Modal "Solicitar ajuste"
- [ ] 5.5 Tela "Aprovações pendentes" (gestor) — lista com aprovar/rejeitar inline
- [ ] 5.6 Tela "Banco de horas" (saldo + movimentos)
- [ ] 5.7 Tela CRUD "Políticas de banco de horas"
- [ ] 5.8 Tela "Fechamento de competência" (RH) — wizard 3 passos
- [ ] 5.9 Notificação no bell para ajustes aprovados/rejeitados
- [ ] 5.10 Permission guards em todas as telas

## Fase 6 — Notificações

- [ ] 6.1 Template e-mail: "Sua solicitação de ajuste foi aprovada"
- [ ] 6.2 Template e-mail: "Espelho mensal disponível"
- [ ] 6.3 Template e-mail: "Você tem ajustes pendentes para aprovar" (digest diário ao gestor)

## Fase 7 — Testes e validação

- [ ] 7.1 Unit: 20 fixtures do engine — cobertura 90%+
- [ ] 7.2 Integration: ciclo completo (bater → ajustar → aprovar → fechar → PDF)
- [ ] 7.3 Integration: hash chain — adulterar 1 linha no DB → job detecta
- [ ] 7.4 Integration: fechar competência com 100 funcionários → todos PDFs no S3 em < 60s
- [ ] 7.5 Convention tests passam
- [ ] 7.6 `openspec validate rh-ponto-interno --strict` válido
- [ ] 7.7 Docs: `documentacao/rh/ponto-interno.md` (operacional) + `politicas-banco-horas.md`
