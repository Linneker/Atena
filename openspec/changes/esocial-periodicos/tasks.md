# Tasks — esocial-periodicos

## Fase 1 — Estrutura
- [ ] 1.1 Pastas `Eventos/V1_2/Periodicos/S1200/`, `S1210/`, `S1299/`, `S3000/`

## Fase 2 — S-1200 Remuneração
- [ ] 2.1 POCO + sub-records
- [ ] 2.2 S1200Builder (mapeia HoleriteFuncionario)
- [ ] 2.3 Mapper rubrica → codRubr eSocial (usa natureza_esocial)
- [ ] 2.4 Validator XSD
- [ ] 2.5 Command `GerarS1200ParaCompetenciaEmLote`
- [ ] 2.6 Hook em FolhaMensal.Fechada
- [ ] 2.7 Tests builder + integration

## Fase 3 — S-1210 Pagamentos
- [ ] 3.1 POCO + Builder
- [ ] 3.2 Hook em ContaPagarFoiPagaEvent (do W10)
- [ ] 3.3 Tests

## Fase 4 — S-1299 Fechamento
- [ ] 4.1 POCO + Builder
- [ ] 4.2 Validação pré-envio (todos S-1200 + S-1210 Aceito)
- [ ] 4.3 Command FecharCompetenciaPeriodicos
- [ ] 4.4 Tests

## Fase 5 — S-3000 Exclusão
- [ ] 5.1 POCO + Builder
- [ ] 5.2 Command ExcluirEventoEsocial
- [ ] 5.3 Command ReabrirCompetencia (S-3000 do S-1299)
- [ ] 5.4 Tests

## Fase 6 — Retificação
- [ ] 6.1 Command RetificarEventoEsocial (genérico)
- [ ] 6.2 OrquestradorRetificacaoCompetencia (encadeia S-1200 retif + S-1210 retif + S-1299 reaberto + re-fechado)
- [ ] 6.3 Tests

## Fase 7 — Orquestração da competência
- [ ] 7.1 OrquestradorPeriodicos.StatusAsync
- [ ] 7.2 Query ObterStatusCompetencia
- [ ] 7.3 Endpoint dashboard

## Fase 8 — Frontend
- [ ] 8.1 Tela "Competência eSocial" — status + ações
- [ ] 8.2 Wizard "Fechar competência" (com checklist pre-envio)
- [ ] 8.3 Tela "Retificar evento"
- [ ] 8.4 Tela "Reabrir competência" (com confirmação dupla)

## Fase 9 — Testes
- [ ] 9.1 Smoke: ciclo completo 5 funcs em Restrita (S-1200 + S-1210 + S-1299 → Aceito)
- [ ] 9.2 Retificação S-1200 funciona end-to-end
- [ ] 9.3 S-3000 exclui evento
- [ ] 9.4 Reabertura via S-3000 do S-1299
- [ ] 9.5 `openspec validate esocial-periodicos --strict` válido
- [ ] 9.6 Docs `documentacao/rh/esocial-periodicos.md` + `esocial-retificacao.md`
