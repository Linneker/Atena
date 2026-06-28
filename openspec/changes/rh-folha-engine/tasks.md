# Tasks — rh-folha-engine

## Fase 1 — Modelo
- [ ] 1.1 Migration `AddTabelaFolhasMensais`
- [ ] 1.2 Migration `AddTabelaHoleritesFuncionarios`
- [ ] 1.3 Migration `AddTabelaErrosCalculoFolha`
- [ ] 1.4 Migration `AddTabelaParametrosCalculoFolha`
- [ ] 1.5 Domain entities + enums (TipoFolha, StatusFolha, TipoRubrica)
- [ ] 1.6 Repos

## Fase 2 — Contexto e helpers

- [ ] 2.1 `ContextoFuncionarioFolha` (POCO)
- [ ] 2.2 `ResumoApontamentos` derivado de marcações W2
- [ ] 2.3 `PreparadorContextoFolha.PrepararAsync(funcId, comp)` — agrega cadastro+apontamento+tabelas+rubricas
- [ ] 2.4 Helper `SalarioBaseProporcional` (admissão no meio do mês)
- [ ] 2.5 Helper `HoraExtra50, HoraExtra100`
- [ ] 2.6 Helper `AdicionalNoturno` (22h-5h, fator 20%)
- [ ] 2.7 Helper `Periculosidade` (30% sobre sal-base)
- [ ] 2.8 Helper `Insalubridade` (10/20/40% sobre SM ou sal-base — conforme grau)
- [ ] 2.9 Helper `DescontoFaltas` e `DescontoAtrasos`
- [ ] 2.10 Helper `DscDsrSobreHe`
- [ ] 2.11 Helper `SalarioFamilia` (calcula por dependente elegível)
- [ ] 2.12 Helper `AplicaTabelaInss` (escalonado, com teto)
- [ ] 2.13 Helper `AplicaTabelaIrrf` (com dependentes e parcela a deduzir)
- [ ] 2.14 Helper `DescontoVT` (min 6% salbase, vlrBenefico)

## Fase 3 — Engine principal

- [ ] 3.1 `EngineFolhaMensal.CalcularAsync(funcId, comp, folhaId)`
- [ ] 3.2 Resolver rubricas de ofício
- [ ] 3.3 Resolver benefícios
- [ ] 3.4 Resolver rubricas custom (DSL via W5 evaluator)
- [ ] 3.5 Topological sort de rubricas custom
- [ ] 3.6 Consolidar bases e totais
- [ ] 3.7 Persistir HoleriteFuncionario
- [ ] 3.8 Tratamento de erro (persiste em ErrosCalculoFolha sem derrubar lote)

## Fase 4 — API e workflow

- [ ] 4.1 Command `AbrirFolha` (5 arquivos)
- [ ] 4.2 Command `CalcularFolha` (enfileira mensagens)
- [ ] 4.3 Worker `CalculoFolhaWorker`
- [ ] 4.4 Command `ConferirFolha`
- [ ] 4.5 Command `FecharFolha`
- [ ] 4.6 Command `ReabrirFolha` (admin)
- [ ] 4.7 Command `RecalcularHoleriteIndividual`
- [ ] 4.8 Queries: status, holerites, totais, divergências
- [ ] 4.9 Endpoints listados em proposal

## Fase 5 — Holerite PDF

- [ ] 5.1 `GeradorHoleritePdf` (QuestPDF) com layout brasileiro padrão
- [ ] 5.2 Worker `HoleritePdfWorker` (RabbitMQ — paralelo após fechamento)
- [ ] 5.3 Endpoint `GET /folha/{id}/holerites/{funcId}.pdf` (síncrono individual)
- [ ] 5.4 Disparo em massa após fechamento + notificação por e-mail
- [ ] 5.5 Mobile: app W3 baixa holerite após fechamento

## Fase 6 — Frontend

- [ ] 6.1 Tela "Folha de pagamento" — listar folhas por competência
- [ ] 6.2 Tela "Abrir folha" — wizard
- [ ] 6.3 Tela "Conferência" — listar holerites + filtros + ações inline (recalcular, ajustar rubrica)
- [ ] 6.4 Tela "Detalhe do holerite" — preview + download PDF
- [ ] 6.5 Tela "Divergências" — lista de erros + ação resolver
- [ ] 6.6 Tela "Totais da folha" — sumário consolidado
- [ ] 6.7 Botão "Fechar folha" com confirmação (irreversível)
- [ ] 6.8 Mobile: tela "Meus holerites" no W3

## Fase 7 — Fixtures e validação

- [ ] 7.1 Criar 30 fixtures (cenários listados em design)
- [ ] 7.2 Cada fixture: input.json + expected.json
- [ ] 7.3 Teste parameterizado roda 30 fixtures × engine
- [ ] 7.4 5 holerites validados manualmente com contador (acompanhamento documentado)
- [ ] 7.5 Integration: folha 100 funcionários em < 3min
- [ ] 7.6 Integration: erro em 1 funcionário não derruba os outros 99
- [ ] 7.7 Property test: total_prov - total_desc == liquido

## Fase 8 — Documentação

- [ ] 8.1 `documentacao/rh/folha-engine.md` (algoritmo completo)
- [ ] 8.2 `documentacao/rh/folha-fixtures.md` (catálogo de fixtures)
- [ ] 8.3 `documentacao/rh/holerite-layout.md`
- [ ] 8.4 `openspec validate rh-folha-engine --strict` válido
