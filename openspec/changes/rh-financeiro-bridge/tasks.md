# Tasks — rh-financeiro-bridge

## Fase 1 — Modelo + extensão Empresa
- [ ] 1.1 Migration `AddTabelaLancamentosFinanceirosFolha`
- [ ] 1.2 Migration `AddTabelaRateiosCentroCustoFolha` (futuro multi-CC)
- [ ] 1.3 Migration `AlterarEmpresasAdicionarRatFapTerceiros` (rat_pct, fap_pct, terceiros_pct, dia_pagamento, cnae_codigo)
- [ ] 1.4 Migration `AlterarContaPagarAdicionarOrigemFolhaId` (FK opcional)
- [ ] 1.5 Domain `LancamentoFinanceiroFolha` + enum `TipoLancamentoFolha`, `StatusLancamento`

## Fase 2 — Helpers e gerador
- [ ] 2.1 `VencimentosFolha.Gps/DarfIrrf/GrfFgts/GrrfFgts/Liquido`
- [ ] 2.2 `ProximoDiaUtil(data, uf?)` reutilizando feriados W5
- [ ] 2.3 `GeradorLancamentosFinanceirosFolha.GerarAsync(folhaId)`
- [ ] 2.4 Agregadores: GpsAggregator, DarfAggregator, GrfAggregator, GrrfHandler

## Fase 3 — Integração com Financeiro
- [ ] 3.1 Estender `IContaPagarService.CriarAsync(...)` para aceitar `origemFolhaId`
- [ ] 3.2 Evento `ContaPagarFoiPagaEvent` publicado em pagamento
- [ ] 3.3 Listener `AtualizaLancamentoFolhaQuandoContaPagarPaga`
- [ ] 3.4 Listener envia notificação ao funcionário (LiquidoFuncionario)

## Fase 4 — Comandos
- [ ] 4.1 Command `GerarLancamentosFinanceirosFolha`
- [ ] 4.2 Command `CancelarLancamentosFolha` (em reabertura)
- [ ] 4.3 Trigger automático: `FolhaMensal.Fechada` → publish event → handler chama Gerar
- [ ] 4.4 Trigger automático: `FolhaMensal.Reaberta` → handler chama Cancelar

## Fase 5 — Queries/Endpoints
- [ ] 5.1 Query `ListarLancamentosFolha(folhaId)`
- [ ] 5.2 Query `ConciliacaoFolha(folhaId)` — % pago vs pendente
- [ ] 5.3 Query `PagamentosVsFolhaPorCompetencia` — relatório
- [ ] 5.4 Endpoints

## Fase 6 — Frontend
- [ ] 6.1 Aba "Lançamentos" no detalhe da Folha
- [ ] 6.2 Tela "Conciliação Folha" — vê pendentes/pagos
- [ ] 6.3 Atalho "Pagar tudo" → seleção em massa de ContasPagar
- [ ] 6.4 Relatório "Pagamentos × Folha" (CSV/PDF)
- [ ] 6.5 Tela "Configuração Tributária Empresa" (RAT, FAP, Terceiros, dia pagamento)

## Fase 7 — Testes
- [ ] 7.1 Unit: agregadores INSS/IRRF/FGTS com 5 cenários cada
- [ ] 7.2 Unit: VencimentosFolha em feriados/finsdesemana
- [ ] 7.3 Integration: folha 10 funcs fechada → 10+3 ContaPagar criadas
- [ ] 7.4 Integration: reabrir folha cancela
- [ ] 7.5 Integration: pagar ContaPagar atualiza lançamento + notifica
- [ ] 7.6 Integration: idempotência (chamar GerarAsync 2x não duplica)
- [ ] 7.7 `openspec validate rh-financeiro-bridge --strict` válido
- [ ] 7.8 Docs `documentacao/rh/financeiro-bridge.md`
