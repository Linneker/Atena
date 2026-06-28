# Tasks — rh-cct-engine

## Fase 1 — Modelo
- [ ] 1.1 Migration `AddTabelaConvencoes`
- [ ] 1.2 Migration `AddTabelaRegrasConvencao`
- [ ] 1.3 Migration `AddTabelaAdesoesConvencao`
- [ ] 1.4 Migration `AddTabelaOverridesConvencaoFuncionario`
- [ ] 1.5 Domain: `Convencao`, `RegraConvencao`, `AdesaoConvencao`, `OverrideConvencaoFuncionario` + enums

## Fase 2 — Tipos de regra + handlers

- [ ] 2.1 Enum `TipoRegraConvencao` (15 valores listados em proposal)
- [ ] 2.2 Param records para cada tipo (PisoSalarialParam, AdicionalHeDiurnoPctParam, AnuenieParam, ...)
- [ ] 2.3 `IRegraConvencaoHandler<T>` interface
- [ ] 2.4 Handler: `HandlerPisoSalarial`
- [ ] 2.5 Handler: `HandlerPisoSalarialPorCbo`
- [ ] 2.6 Handler: `HandlerAdicionalHeDiurnoPct`
- [ ] 2.7 Handler: `HandlerAdicionalHeNoturnoPct`
- [ ] 2.8 Handler: `HandlerAdicionalNoturnoPct`
- [ ] 2.9 Handler: `HandlerPericulosidadePct`
- [ ] 2.10 Handler: `HandlerInsalubridadeGrau`
- [ ] 2.11 Handler: `HandlerAnueniePct`
- [ ] 2.12 Handler: `HandlerAdicionalTempoServico`
- [ ] 2.13 Handler: `HandlerValeAlimentacao`
- [ ] 2.14 Handler: `HandlerAuxilioCreche`
- [ ] 2.15 Handler: `HandlerMultaRescisao`
- [ ] 2.16 Handler: `HandlerAvisoPrevioDias`
- [ ] 2.17 Handler: `HandlerGatilhoReajuste`
- [ ] 2.18 Handler: `HandlerRegraCustomDsl` (chama W5 evaluator)
- [ ] 2.19 `RegraConvencaoHandlerRegistry` (DI scan + dispatch por tipo)

## Fase 3 — Resolvedor

- [ ] 3.1 `IResolvedorConvencao.ResolverAsync(funcId, competencia)`
- [ ] 3.2 Lógica: override > adesão > nenhum
- [ ] 3.3 Cache de resolução (TTL curto, invalidado em mudança)

## Fase 4 — Integração com W6

- [ ] 4.1 `ContextoFuncionarioFolha` ganha `Convencao? Convencao`, `decimal PctHeDiurno`, `decimal PctNoturno`, etc.
- [ ] 4.2 `EngineFolhaMensal` chama `ResolverConvencao` e aplica handlers
- [ ] 4.3 Helpers W6 (HoraExtra50 etc) leem `ctx.PctHeDiurno` em vez de hardcoded
- [ ] 4.4 Recalcular fixtures W6 com CCT cenários

## Fase 5 — API

- [ ] 5.1 CRUD `Convencao` (5 verticals + endpoints)
- [ ] 5.2 CRUD `RegraConvencao` aninhado
- [ ] 5.3 Command/Query `AderirEmpresaAConvencao`, `RetirarAdesao`
- [ ] 5.4 Command `DefinirOverrideConvencaoFuncionario`
- [ ] 5.5 Query `ListarConvencoesVigentes`
- [ ] 5.6 Command `SimularImpactoConvencao` (assíncrono)
- [ ] 5.7 Query `ResultadoSimulacaoImpacto`
- [ ] 5.8 Endpoints
- [ ] 5.9 Worker `SimulacaoImpactoCctWorker`

## Fase 6 — Detecção de afetação

- [ ] 6.1 Trigger ao salvar Convencao/Regra/Adesão → marca folhas afetadas com `precisa_recalcular`
- [ ] 6.2 Notificação no bell para RH

## Fase 7 — Frontend

- [ ] 7.1 Tela "Convenções" (CRUD)
- [ ] 7.2 Tela "Regras da CCT" (CRUD aninhado) com construtor visual
- [ ] 7.3 Form específico por tipo de regra (dropdown + fields por schema)
- [ ] 7.4 Tela "Adesões" (empresa × convenção)
- [ ] 7.5 Tela "Override por funcionário"
- [ ] 7.6 Tela "Simular impacto" com diff visual de holerites
- [ ] 7.7 Importar CCT por upload JSON

## Fase 8 — Testes

- [ ] 8.1 Unit handler por tipo (15 × 3 cenários)
- [ ] 8.2 Unit resolvedor (5 cenários)
- [ ] 8.3 Fixtures de CCT em `documentacao/rh/cct/exemplos/`
- [ ] 8.4 Integration: criar CCT METAL-SP com 3 regras → aplicar em 5 funcs → expected confere
- [ ] 8.5 Integration: simular impacto → diff confere com cálculo manual
- [ ] 8.6 Integration: mudança em CCT afetando folha calculada marca recálculo
- [ ] 8.7 `openspec validate rh-cct-engine --strict` válido
- [ ] 8.8 Docs `documentacao/rh/cct.md` + `documentacao/rh/cct/tipos-de-regra.md`
