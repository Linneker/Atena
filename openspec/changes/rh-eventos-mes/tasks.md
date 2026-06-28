# Tasks — rh-eventos-mes

## Fase 1 — Modelo
- [ ] 1.1 Migration `AddTabelaFerias`
- [ ] 1.2 Migration `AddTabelaDecimo3o`
- [ ] 1.3 Migration `AddTabelaAdiantamentos`
- [ ] 1.4 Migration `AddTabelaAfastamentos`
- [ ] 1.5 Domain + enums

## Fase 2 — Engines especializados
- [ ] 2.1 `EngineFolhaFerias`
- [ ] 2.2 `EngineFolha13oParcela1`
- [ ] 2.3 `EngineFolha13oParcela2`
- [ ] 2.4 `EngineFolhaAdiantamento`
- [ ] 2.5 Helpers: dias_direito_ferias_por_faltas, meses_trabalhados, salario_diario

## Fase 3 — Período aquisitivo / Jobs
- [ ] 3.1 Job `JobAtualizarPeriodoAquisitivoFerias` (mensal)
- [ ] 3.2 Job `JobAlertaFeriasVencendo` (diário)
- [ ] 3.3 Job `JobProcessar13o1aParcela` (15/nov ou manual)
- [ ] 3.4 Job `JobProcessar13o2aParcela` (15/dez ou manual)

## Fase 4 — Commands/Queries/Endpoints (CRUD + ações)
- [ ] 4.1 Ferias: ListarSaldo, ProgramarFerias, CancelarFerias, MarcarGozado
- [ ] 4.2 13º: PreviewDecimo3o, Processar1aParcela, Processar2aParcela
- [ ] 4.3 Adiantamentos: CriarAdiantamento, GerarFolhaAdiantamentos
- [ ] 4.4 Afastamentos: RegistrarAfastamento, EncerrarAfastamento, ListarAlertas15Dias
- [ ] 4.5 Endpoints todos

## Fase 5 — Documentos
- [ ] 5.1 PDF Aviso de Férias (QuestPDF)
- [ ] 5.2 PDF Recibo 13º
- [ ] 5.3 PDF Comprovante de Afastamento

## Fase 6 — Frontend
- [ ] 6.1 Tela "Férias" lista + saldo por funcionário
- [ ] 6.2 Wizard "Programar férias"
- [ ] 6.3 Tela "13º" preview + processar
- [ ] 6.4 Tela "Adiantamentos"
- [ ] 6.5 Tela "Afastamentos"
- [ ] 6.6 Calendário de afastamentos
- [ ] 6.7 Alertas no bell

## Fase 7 — Testes
- [ ] 7.1 15 fixtures (ferias normal, vende 10, adto 13, abono, 13 6m, 13 9m com aumento, ...)
- [ ] 7.2 Integration: programar férias → aviso PDF + folha avulsa correta
- [ ] 7.3 Integration: 13º com 50 funcionários
- [ ] 7.4 Integration: job de pendência cria férias automaticamente
- [ ] 7.5 `openspec validate rh-eventos-mes --strict` válido
- [ ] 7.6 Docs `documentacao/rh/eventos-mes.md`
